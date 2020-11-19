using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Data;
using Capstone.Models;
using Capstone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var pendingAppointments = _context.PendingAppointments.ToList();

            ViewData["TodaysAppointments"] = _context.Appointments.Where(a => a.ServiceStart.Date == DateTime.Now.Date).ToList();
            ViewData["UpcomingAppointments"] = _context.Appointments.Where(a => a.ServiceStart.Date > DateTime.Now.Date).ToList();

            return View(pendingAppointments);
        }

        public ActionResult Clients()
        {
            return View();
        }

        public ActionResult CreateRuleSet()
        {
            return View();
        }

        [BindProperty]
        public List<string> Days { get; set; }
        [BindProperty]
        public List<string> Times { get; set; }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRuleSet(RuleSet ruleSet)
        {
            _context.RuleSets.Add(ruleSet);
            _context.SaveChanges();

            foreach (string day in Days)
            {
                foreach (string time in Times)
                {
                    AppointmentBlock appointmentBlock = new AppointmentBlock { DayOfWeek = day, StartTime = time, RuleSet = _context.RuleSets.Where(rs => rs.RuleSetId == ruleSet.RuleSetId).SingleOrDefault(), RuleSetId = ruleSet.RuleSetId };
                    _context.AppointmentBlocks.Add(appointmentBlock);
                }
            }
            _context.SaveChanges();

            return View();
        }

        public ActionResult ViewRuleSets()
        {
            var ruleSets = _context.RuleSets.ToList();
            return View(ruleSets);
        }

        public ActionResult PendingAppointmentDetails(int id)
        {
            var appointment = _context.PendingAppointments.Where(pa => pa.PendingAppointmentId == id).SingleOrDefault();

            CreateNewAppointmentViewModel model = new CreateNewAppointmentViewModel() {
                PendingAppoitmentId = id,
                Make = appointment.PianoMake,
                Configuration = appointment.PianoConfiguration,
                FirstName = appointment.FirstName,
                LastName = appointment.LastName,
                Email = appointment.Email,
                Phone = appointment.Phone,
                StreetAddress = appointment.StreetAddress,
                City = appointment.City,
                State = appointment.State,
                ZipCode = appointment.ZipCode,
                IncludedServices = appointment.IncludedServices,
                CustomerNotes = appointment.CustomerNotes,
                ServiceStart = appointment.ServiceStart,
                ServiceEnd = appointment.ServiceEnd,
            };

            var matchingClients = _context.Clients.Include(c => c.Address).Where(c => (c.FirstName == appointment.FirstName) && (c.LastName == appointment.LastName)).ToList();
            List<int> clientIds = new List<int>();
            foreach (Client client in matchingClients)
            {
                clientIds.Add(client.ClientId);
            }
            ViewBag.matchingPianos = _context.Pianos.Where(p => clientIds.Contains(p.ClientId)).ToList();
            ViewBag.matchingClients = matchingClients;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointment(CreateNewAppointmentViewModel model)
        {
            model.Latitude = 0;
            model.Longitude = 0;
            int clientId = model.ClientId;
            int pianoId = model.PianoId;
            if (!ModelState.IsValid)
            {
                if (model.ClientId == 0 || model.PianoId == 0)
                {
                    if (model.ClientId == 0)
                    {
                        // Create new address instance
                        Address address = new Address()
                        {
                            StreetAddress = model.StreetAddress,
                            City = model.City,
                            State = model.State,
                            ZipCode = model.ZipCode
                            // Geocode Address
                        };
                        _context.Addresses.Add(address);
                        _context.SaveChanges();

                        // Create new client instance
                        Client client = new Client()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Phone = model.Phone,
                            AddressId = address.AddressId
                        };
                        _context.Clients.Add(client);
                        _context.SaveChanges();
                        clientId = client.ClientId;
                    }

                    if (model.PianoId == 0)
                    {
                        // Create new piano instance
                        Piano piano = new Piano()
                        {
                            ClientId = clientId,
                            Make = model.Make,
                            Configuration = model.Configuration,
                        };
                        _context.Pianos.Add(piano);
                        _context.SaveChanges();
                        pianoId = piano.PianoId;
                    }
                }
                else
                {
                    return View("PendingAppointmentDetails", model);
                }
            }

            Appointment appointment = new Appointment()
            {
                PianoId = pianoId,
                Piano = _context.Pianos.Where(p => p.PianoId == pianoId).SingleOrDefault(),
                IncludedServices = model.IncludedServices,
                CustomerNotes = model.CustomerNotes,
                ServiceStart = model.ServiceStart,
                ServiceEnd = model.ServiceEnd
            };

            _context.Appointments.Add(appointment);
            _context.PendingAppointments.Remove(_context.PendingAppointments.Where(pa => pa.PendingAppointmentId == model.PendingAppoitmentId).SingleOrDefault());
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}