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

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public GoogleService _google;
        public MailKitService _mailKit;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, GoogleService google, MailKitService mailKit)
        {
            _context = context;
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

        public IActionResult Schedule()
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
            pendingAppointment.Services = string.Join(", ", ChosenServices.ToArray());
            foreach(var includedService in ChosenServices)
            {
                foreach(var service in PianoServices.TuningServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        pendingAppointment.EstimatedCost += service.Cost;
                        break;
                    }
                }
                foreach(var service in PianoServices.RepairServices)
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

            // Geocode address
            pendingAppointment = await _google.GeocodeAddress(pendingAppointment);

            ViewBag.availableAppointments = await GetAppointments(pendingAppointment);
            
            // Logic for checking database for available appointments
            // Bind appointments to ViewBag
            return View(pendingAppointment);
        }

        public IActionResult ConfirmRequest(PendingAppointment pendingAppointment)
        {
            TimeSpan duration = new TimeSpan(0, pendingAppointment.EstimatedDuration, 0);
            pendingAppointment.ServiceEnd = pendingAppointment.ServiceStart + duration;
            _context.PendingAppointments.Add(pendingAppointment);
            _context.SaveChanges();
            _mailKit.ConfirmAppointmentRequest(pendingAppointment);

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
                var currentRuleSet = _context.RuleSets.Where(rs => rs.StartDate <= currentDay.Date).OrderByDescending(rs => rs.StartDate).FirstOrDefault();

                // Find all existing appointments for preferred appointment date
                var existingAppointments = _context.Appointments.Where(a => a.ServiceStart.Date == currentDay.Date).ToList();

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
                            var appointmentBlock = _context.AppointmentBlocks.Where(ab => (ab.RuleSetId == currentRuleSet.RuleSetId) && (ab.Day == currentDay.DayOfWeek)).SingleOrDefault();
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
                    var defaultTimes = _context.DefaultTimes.Where(dt => dt.RuleSetId == currentRuleSet.RuleSetId).ToList();

                    // Find appointment block in current rule set for correct day of week
                    var appointmentBlock = _context.AppointmentBlocks.Where(ab => (ab.RuleSetId == currentRuleSet.RuleSetId) && (ab.Day == currentDay.DayOfWeek)).SingleOrDefault();

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
    }
}
