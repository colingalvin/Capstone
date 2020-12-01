using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Capstone.Models;
using Capstone.Data;
using Capstone.Services;
using Capstone.Contracts;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepositoryWrapper _repo;
        public GoogleService _google;
        public MailKitService _mailKit;

        public HomeController(IRepositoryWrapper repo, ILogger<HomeController> logger, GoogleService google, MailKitService mailKit)
        {
            _repo = repo;
            _logger = logger;
            _google = google;
            _mailKit = mailKit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult ScheduleService()
        {
            return View();
        }

        [BindProperty]
        public List<string> ChosenServices { get; set; }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseAppointmentTime(PendingAppointment pendingAppointment)
        {
            pendingAppointment = CalculateServiceTimeAndCost(pendingAppointment, ChosenServices);
            pendingAppointment = await _google.GeocodeAddress(pendingAppointment);
            ViewBag.availableAppointments = await GetAppointments(pendingAppointment);
            return View(pendingAppointment);
        }

        public IActionResult ConfirmRequest(PendingAppointment pendingAppointment)
        {
            pendingAppointment.ServiceEnd = pendingAppointment.ServiceStart + new TimeSpan(0, pendingAppointment.EstimatedDuration, 0);
            _repo.PendingAppointment.Create(pendingAppointment);
            _repo.Save();
            _mailKit.SendAppointmentRequestEmail(pendingAppointment);

            return View(pendingAppointment);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<DateTime>> GetAppointments(PendingAppointment pendingAppointment)
        {
            List<DateTime> availableAppointments = new List<DateTime>();

            // Start with preferred appointment day
            DateTime currentDay = pendingAppointment.PreferredAppointmentDate;

            // Need logic if rule set does not exist
            
            do
            {
                // Find relevant rule set
                var currentRuleSet = _repo.RuleSet.FindByCondition(rs => (rs.StartDate <= currentDay.Date) && ((rs.EndDate == null) || (rs.EndDate > currentDay.Date))).OrderByDescending(rs => rs.StartDate).FirstOrDefault();

                // Find all existing appointments for preferred appointment date
                var existingAppointments = _repo.Appointment.FindByCondition(a => a.ServiceStart.Date == currentDay.Date).ToList();

                // If appointments exist
                if (existingAppointments.Count > 0)
                {
                    // Calculate if any pending appointment is within range
                    foreach (Appointment appointment in existingAppointments)
                    {
                        // Calculate drive time from each existing appointment to pending appointment
                        var driveTime = await _google.GetTravelTime(appointment.Latitude, appointment.Longitude, pendingAppointment.Latitude, pendingAppointment.Longitude);

                        //If drive time is within drive time limitatons
                        if (TimeSpan.Compare(driveTime, DefaultSettings.maxDriveTime) <= 0)
                        {
                            // Calculate start time of new appointment, rounded up to nearest quarter hour
                            DateTime startTime = CalculateServiceStart(appointment.ServiceEnd + driveTime, TimeSpan.FromMinutes(15));

                            // Calculate estimated end time of new appointment from estimated start time
                            DateTime endTime = startTime + new TimeSpan(0, pendingAppointment.EstimatedDuration, 0);


                            // Find appointment block in current rule set for correct day of week
                            var appointmentBlock = _repo.AppointmentBlock.FindByCondition(ab => (ab.RuleSetId == currentRuleSet.RuleSetId) && (ab.Day == currentDay.DayOfWeek)).SingleOrDefault();
                            var appointmentBlockEndTime = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, appointmentBlock.EndTime.Hour, appointmentBlock.EndTime.Minute, appointmentBlock.EndTime.Second);

                            // If appointment will end before the end of the work day
                            if (DateTime.Compare(endTime, appointmentBlockEndTime) <= 1)
                            {
                                // Add new appointment time to list with start time calculated from end time of appointment rounded to nearest 15 minutes
                                var newAppointmentTime = new DateTime(
                                    currentDay.Year,
                                    currentDay.Month,
                                    currentDay.Day,
                                    startTime.Hour,
                                    startTime.Minute,
                                    startTime.Second
                                );
                                availableAppointments.Add(newAppointmentTime);
                            };
                        };
                    }
                }
                else
                {
                    // Find default times
                    var defaultTimes = _repo.DefaultTime.FindByCondition(dt => dt.RuleSetId == currentRuleSet.RuleSetId).ToList();

                    // Find appointment block in current rule set for correct day of week
                    var appointmentBlock = _repo.AppointmentBlock.FindByCondition(ab => (ab.RuleSetId == currentRuleSet.RuleSetId) && (ab.Day == currentDay.DayOfWeek)).SingleOrDefault();

                    if (appointmentBlock != null)
                    {
                        // Create appointments based on default appointment times
                        foreach (DefaultTime time in defaultTimes)
                        {
                            var defaultTime = new DateTime(
                                currentDay.Year,
                                currentDay.Month,
                                currentDay.Day,
                                time.StartTime.Hour,
                                time.StartTime.Minute,
                                time.StartTime.Second
                                );
                            availableAppointments.Add(defaultTime);
                        };
                    }
                }
                currentDay += new TimeSpan(24, 0, 0);
            }
            while (availableAppointments.Count < 10);
            return availableAppointments;
        }
        
        private DateTime CalculateServiceStart(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        private PendingAppointment CalculateServiceTimeAndCost(PendingAppointment pendingAppointment, List<string> includedServices)
        {
            pendingAppointment.Services = string.Join(", ", includedServices.ToArray());
            foreach (var includedService in includedServices)
            {
                foreach (var service in PianoServices.TuningServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        pendingAppointment.EstimatedCost += service.Cost;
                        break;
                    }
                }
                foreach (var service in PianoServices.RepairServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        pendingAppointment.EstimatedCost += service.Cost;
                        break;
                    }
                }
                foreach (var service in PianoServices.CleaningServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        pendingAppointment.EstimatedCost += service.Cost;
                        break;
                    }
                }
            }
            return pendingAppointment;
        }
    }
}
