using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Capstone.Models;
using Capstone.Data;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
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
        public List<string> IncludedServices { get; set; }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult ChooseAppointmentTime(PendingAppointment pendingAppointment)
        {
            pendingAppointment.IncludedServices = string.Join(", ", IncludedServices.ToArray());
            foreach(var includedService in IncludedServices)
            {
                foreach(var service in PianoServices.TuningServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        break;
                    }
                }
                foreach(var service in PianoServices.RepairServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        break;
                    }
                }
                foreach (var service in PianoServices.CleaningServices)
                {
                    if (includedService == service.Name)
                    {
                        pendingAppointment.EstimatedDuration += service.Time;
                        break;
                    }
                }
            }
            // Geocode address
            pendingAppointment.City = "O'Fallon";
            pendingAppointment.State = "IL";
            pendingAppointment.Latitude = 38.583220;
            pendingAppointment.Longitude = -89.906720;

            // Logic for checking database for available appointments
            // Bind appointments to ViewBag
            return View(pendingAppointment);
        }

        public IActionResult ConfirmRequest(PendingAppointment pendingAppointment)
        {
            return View(pendingAppointment);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
