using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Contracts;
using Capstone.Data;
using Capstone.Models;
using Capstone.Services;
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
        public MailKitService _mailKitService;
        private IRepositoryWrapper _repo;

        public AdminController(MailKitService mailKitService, IRepositoryWrapper repo)
        {
            _mailKitService = mailKitService;
            _repo = repo;
        }
        public ActionResult Index()
        {
            CheckForReminderEmails();
            var pendingAppointments = _repo.PendingAppointment.FindAll().ToList();//_context.PendingAppointments.ToList();

            ViewBag.completedAppointments = _repo.Appointment.FindByCondition(a => (a.ServiceEnd < DateTime.Now) && (a.IsComplete == false)).Include(a => a.Piano.Client.Address).ToList();
            //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => (a.ServiceEnd < DateTime.Now) && (a.IsComplete == false)).ToList();
            ViewBag.todaysAppointments = _repo.Appointment.FindByCondition(a => a.ServiceStart.Date == DateTime.Now.Date).Include(a => a.Piano.Client.Address).ToList();
            //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => a.ServiceStart.Date == DateTime.Now.Date).ToList();
            ViewBag.nextSevenDaysAppointments = _repo.Appointment.FindByCondition(a => (a.ServiceStart.Date > DateTime.Now) && (a.ServiceStart.Date < DateTime.Now.AddDays(7).Date)).Include(a => a.Piano.Client.Address).OrderBy(a => a.ServiceStart).ToList();
            //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => (a.ServiceStart.Date > DateTime.Now) && (a.ServiceStart.Date < DateTime.Now.AddDays(7).Date)).OrderBy(a => a.ServiceStart).ToList();
            return View(pendingAppointments);
        }

        public ActionResult AllClients()
        {
            var clients = _repo.Client.FindAll().Include(c => c.Address).ToList();
            //_context.Clients.Include(c => c.Address).ToList();
            return View(clients);
        }

        public ActionResult ServiceHistory(int? id)
        {
            if (id == null)
            {
                var appointments = _repo.Appointment.FindByCondition(a => a.IsComplete == true).Include(a => a.Piano.Client.Address).OrderByDescending(a => a.ServiceEnd).ToList();
                //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => a.IsComplete == true).OrderByDescending(a => a.ServiceEnd).ToList();
                return View(appointments);
            }
            else
            {
                var appointments = _repo.Appointment.FindByCondition(a => (a.IsComplete == true) && (a.Piano.ClientId == id)).Include(a => a.Piano.Client.Address).OrderByDescending(a => a.ServiceEnd).ToList();
                //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => (a.IsComplete == true) && (a.Piano.ClientId == id)).OrderByDescending(a => a.ServiceEnd).ToList();
                return View(appointments);
            }
        }

        public ActionResult EditClient(int id)
        {
            var client = _repo.Client.FindByCondition(c => c.ClientId == id).Include(c => c.Address).SingleOrDefault();
            //_context.Clients.Include(c => c.Address).Where(c => c.ClientId == id).SingleOrDefault();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClient(Client client)
        {
            _repo.Address.Update(client.Address);
            _repo.Client.Update(client);
            _repo.Save();
            return RedirectToAction("AllClients");
        }

        public ActionResult ClientDetails(int id)
        {
            var client = _repo.Client.FindByCondition(c => c.ClientId == id).Include(c => c.Address).SingleOrDefault();
            //_context.Clients.Include(c => c.Address).Where(c => c.ClientId == id).SingleOrDefault();
            ViewBag.pianos = _repo.Piano.FindByCondition(p => p.ClientId == client.ClientId).ToList();
            //_context.Pianos.Where(p => p.ClientId == client.ClientId);
            return View(client);
        }

        public ActionResult EditPiano(int id)
        {
            var piano = _repo.Piano.FindByCondition(p => p.PianoId == id).SingleOrDefault();
            //_context.Pianos.Where(p => p.PianoId == id).SingleOrDefault();
            return View(piano);
        }

        public async Task<ActionResult> CompleteAppointment(int id)
        {
            var appointment = await _repo.Appointment.FindByCondition(a => a.AppointmentId == id).SingleOrDefaultAsync();
            //await _context.Appointments.Where(a => a.AppointmentId == id).SingleOrDefaultAsync();
            var piano = await _repo.Piano.FindByCondition(p => p.PianoId == appointment.PianoId).SingleOrDefaultAsync();
            //await _context.Pianos.Where(p => p.PianoId == appointment.PianoId).SingleOrDefaultAsync();
            appointment.IsComplete = true;
            piano.LastService = appointment.ServiceStart.Date;
            piano.RemindForService = piano.LastService + new TimeSpan(365, 0, 0, 0);
            piano.Reminded = false;
            _repo.Appointment.Update(appointment);
            //_context.Appointments.Update(appointment);
            _repo.Piano.Update(piano);
            //_context.Pianos.Update(piano);
            _repo.Save();
            //_context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditAppointment(int id)
        {
            var appointment = _repo.Appointment.FindByCondition(a => a.AppointmentId == id).Include(a => a.Piano.Client.Address).SingleOrDefault();
            //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => a.AppointmentId == id).SingleOrDefault();
            return View(appointment);
        }

        public ActionResult SaveAppointmentChanges(Appointment alteredAppointment)
        {
            _repo.Appointment.Update(alteredAppointment);
            //_context.Appointments.Update(alteredAppointment);
            _repo.Piano.Update(alteredAppointment.Piano);
            //_context.Pianos.Update(alteredAppointment.Piano);
            _repo.Client.Update(alteredAppointment.Piano.Client);
            //_context.Clients.Update(alteredAppointment.Piano.Client);
            _repo.Address.Update(alteredAppointment.Piano.Client.Address);
            //_context.Addresses.Update(alteredAppointment.Piano.Client.Address);
            _repo.Save();
            //_context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditRuleSet(int? id)
        {
            CreateNewRuleSetViewModel ruleSet;
            if (id != null)
            {
                RuleSet existingRuleSet = _repo.RuleSet.FindByCondition(rs => rs.RuleSetId == id).Include(rs => rs.Address).SingleOrDefault();
                //_context.RuleSets.Include(rs => rs.Address).Where(rs => rs.RuleSetId == id).SingleOrDefault();
                List<AppointmentBlock> existingBlocks = _repo.AppointmentBlock.FindByCondition(ab => ab.RuleSetId == existingRuleSet.RuleSetId).ToList();
                //_context.AppointmentBlocks.Where(ab => ab.RuleSetId == existingRuleSet.RuleSetId).ToList();
                List<DefaultTime> existingDefaultTimes = _repo.DefaultTime.FindByCondition(dt => dt.RuleSetId == existingRuleSet.RuleSetId).ToList();
                //_context.DefaultTimes.Where(dt => dt.RuleSetId == existingRuleSet.RuleSetId).ToList();
                List<DateTime> existingTimes = new List<DateTime>();
                foreach(DefaultTime time in existingDefaultTimes)
                {
                    existingTimes.Add(time.StartTime);
                }
                List<DayOfWeek> days = new List<DayOfWeek>();
                foreach(var block in existingBlocks)
                    {
                        days.Add(block.Day);
                    };
                ruleSet = new CreateNewRuleSetViewModel()
                {
                    RuleSetId = existingRuleSet.RuleSetId,
                    StartDate = existingRuleSet.StartDate,
                    EndDate = existingRuleSet.EndDate,
                    Default = existingRuleSet.Default,
                    Days = days,
                    StartTime = existingBlocks[0].StartTime,
                    EndTime = existingBlocks[0].EndTime,
                    DefaultTimes = existingTimes,
                    StreetAddress = existingRuleSet.Address.StreetAddress,
                    Zip = existingRuleSet.Address.Zip
                };
            }
            else
            {
                ruleSet = new CreateNewRuleSetViewModel() { StartDate = DateTime.Today };
            }
            return View(ruleSet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRuleSet(CreateNewRuleSetViewModel ruleSet)
        {
            if (ruleSet.RuleSetId == 0)
            {
                // Check for existing/create new address
                var matchingAddress = _repo.Address.FindByCondition(a => a.StreetAddress == ruleSet.StreetAddress).SingleOrDefault();
                //_context.Addresses.Where(a => a.StreetAddress == ruleSet.StreetAddress).SingleOrDefault();
                if (matchingAddress == null)
                {
                    Address address = new Address()
                    {
                        StreetAddress = ruleSet.StreetAddress,
                        Zip = ruleSet.Zip
                    };
                    // Geocode/fill in additional Address information
                    _repo.Address.Create(address);
                    //_context.Addresses.Add(address);
                    _repo.Save();
                    //_context.SaveChanges();
                    matchingAddress = address;
                }

                // Create new rule set
                RuleSet newRuleSet = new RuleSet()
                {
                    StartDate = ruleSet.StartDate,
                    EndDate = ruleSet.EndDate,
                    Default = ruleSet.Default,
                    HomeAddressId = matchingAddress.AddressId
                };
                _repo.RuleSet.Create(newRuleSet);
                //_context.RuleSets.Add(newRuleSet);
                _repo.Save();
                //_context.SaveChanges();

                // Create new appointment blocks
                if(ruleSet.Days != null)
                {
                    foreach (var day in ruleSet.Days)
                    {
                        AppointmentBlock appointmentBlock = new AppointmentBlock()
                        {
                            Day = day,
                            StartTime = ruleSet.StartTime,
                            EndTime = ruleSet.EndTime,
                            RuleSetId = newRuleSet.RuleSetId,
                            RuleSet = newRuleSet
                        };
                        _repo.AppointmentBlock.Create(appointmentBlock);
                        //_context.AppointmentBlocks.Add(appointmentBlock);
                    }

                    // Create new default appointment times
                    foreach (var time in ruleSet.DefaultTimes)
                    {
                        DefaultTime defaultTime = new DefaultTime()
                        {
                            StartTime = time,
                            RuleSetId = newRuleSet.RuleSetId,
                            RuleSet = newRuleSet
                        };
                        _repo.DefaultTime.Create(defaultTime);
                        //_context.DefaultTimes.Add(defaultTime);
                    }
                    _repo.Save();
                    //_context.SaveChanges();
                }    
            }
            else
            {
                // Update/save changes to existing rule set
                RuleSet existingRuleSet = _repo.RuleSet.FindByCondition(rs => rs.RuleSetId == ruleSet.RuleSetId).SingleOrDefault();
                //_context.RuleSets.Where(rs => rs.RuleSetId == ruleSet.RuleSetId).SingleOrDefault();
                existingRuleSet.StartDate = ruleSet.StartDate;
                existingRuleSet.EndDate = ruleSet.EndDate;
                existingRuleSet.Default = ruleSet.Default;
                _repo.RuleSet.Update(existingRuleSet);
                //_context.RuleSets.Update(existingRuleSet);

                // Check for changes to address
                Address address = _repo.Address.FindByCondition(a => a.AddressId == existingRuleSet.HomeAddressId).SingleOrDefault();
                //_context.Addresses.Where(a => a.AddressId == existingRuleSet.HomeAddressId).SingleOrDefault();
                if (existingRuleSet.Address.StreetAddress != address.StreetAddress)
                {
                    Address newAddress = new Address()
                    {
                        StreetAddress = ruleSet.StreetAddress,
                        Zip = ruleSet.Zip
                    };
                    // Geocode/fill in additional Address information
                    _repo.Address.Create(newAddress);
                    //_context.Addresses.Add(newAddress);
                    _repo.Save();
                    //_context.SaveChanges();
                    existingRuleSet.HomeAddressId = newAddress.AddressId;
                }

                // Delete all appointment blocks associated with rule set id
                List<AppointmentBlock> blocks = _repo.AppointmentBlock.FindByCondition(ab => ab.RuleSetId == existingRuleSet.RuleSetId).Include(ab => ab.RuleSet).ToList();
                //_context.AppointmentBlocks.Include(ab => ab.RuleSet).Where(ab => ab.RuleSetId == existingRuleSet.RuleSetId).ToList();
                foreach (AppointmentBlock block in blocks)
                {
                    _repo.AppointmentBlock.Delete(block);
                    //_context.AppointmentBlocks.Remove(block);
                }
                _repo.Save();
                //_context.SaveChanges();

                // Create new appointment blocks associated with rule set id
                foreach (DayOfWeek day in ruleSet.Days)
                {
                    AppointmentBlock appointmentBlock = new AppointmentBlock()
                    {
                        Day = day,
                        StartTime = ruleSet.StartTime,
                        EndTime = ruleSet.EndTime,
                        RuleSetId = existingRuleSet.RuleSetId,
                        RuleSet = existingRuleSet
                    };
                    _repo.AppointmentBlock.Create(appointmentBlock);
                    //_context.AppointmentBlocks.Add(appointmentBlock);
                }
                _repo.Save();
                //_context.SaveChanges();

                // Delete all default appointment times associated with rule set id
                var defaultTimes = _repo.DefaultTime.FindByCondition(dt => dt.RuleSetId == existingRuleSet.RuleSetId).Include(dt => dt.RuleSet).ToList();
                //_context.DefaultTimes.Include(dt => dt.RuleSet).Where(dt => dt.RuleSetId == existingRuleSet.RuleSetId).ToList();
                foreach (DefaultTime defaultTime in defaultTimes)
                {
                    _repo.DefaultTime.Delete(defaultTime);
                    //_context.DefaultTimes.Remove(defaultTime);
                }
                _repo.Save();
                //_context.SaveChanges();

                // Create new default appointment times associated with rule set id
                foreach (DateTime time in ruleSet.DefaultTimes)
                {
                    DefaultTime defaultTime = new DefaultTime()
                    {
                        StartTime = time,
                        RuleSetId = existingRuleSet.RuleSetId,
                        RuleSet = existingRuleSet
                    };
                    _repo.DefaultTime.Create(defaultTime);
                    //_context.DefaultTimes.Add(defaultTime);
                    _repo.Save();
                    //_context.SaveChanges();
                }
            }
            return RedirectToAction("ViewRuleSets");
        }

        public ActionResult ViewRuleSets()
        {
            var ruleSets = _repo.RuleSet.FindAll().ToList();
            //_context.RuleSets.ToList();
            return View(ruleSets);
        }

        public ActionResult PendingAppointmentDetails(int id)
        {
            var appointment = _repo.PendingAppointment.FindByCondition(pa => pa.PendingAppointmentId == id).SingleOrDefault();
            //_context.PendingAppointments.Where(pa => pa.PendingAppointmentId == id).SingleOrDefault();

            CreateNewAppointmentViewModel model = new CreateNewAppointmentViewModel() {
                PendingAppointmentId = id,
                Make = appointment.PianoMake,
                Configuration = appointment.PianoConfiguration,
                FirstName = appointment.FirstName,
                LastName = appointment.LastName,
                Email = appointment.Email,
                Phone = appointment.Phone,
                StreetAddress = appointment.StreetAddress,
                City = appointment.City,
                State = appointment.State,
                Zip = appointment.Zip,
                Services = appointment.Services,
                CustomerNotes = appointment.CustomerNotes,
                ServiceStart = appointment.ServiceStart,
                ServiceEnd = appointment.ServiceEnd,
                Latitude = appointment.Latitude,
                Longitude = appointment.Longitude,
                EstimatedCost = appointment.EstimatedCost
            };

            var matchingClients = _repo.Client.FindByCondition(c => (c.FirstName == appointment.FirstName) && (c.LastName == appointment.LastName)).Include(c => c.Address).ToList();
            //_context.Clients.Include(c => c.Address).Where(c => (c.FirstName == appointment.FirstName) && (c.LastName == appointment.LastName)).ToList();
            List<int> clientIds = new List<int>();
            foreach (Client client in matchingClients)
            {
                clientIds.Add(client.ClientId);
            }
            ViewBag.matchingPianos = _repo.Piano.FindByCondition(p => clientIds.Contains(p.ClientId)).ToList();
            //_context.Pianos.Where(p => clientIds.Contains(p.ClientId)).ToList();
            ViewBag.matchingClients = matchingClients;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointment(CreateNewAppointmentViewModel model)
        {
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
                            Zip = model.Zip,
                            Latitude = model.Latitude,
                            Longitude = model.Longitude
                        };
                        _repo.Address.Create(address);
                        //_context.Addresses.Add(address);
                        _repo.Save();
                        //_context.SaveChanges();

                        // Create new client instance
                        Client client = new Client()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Phone = model.Phone,
                            AddressId = address.AddressId
                        };
                        _repo.Client.Create(client);
                        //_context.Clients.Add(client);
                        _repo.Save();
                        //_context.SaveChanges();
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
                        _repo.Piano.Create(piano);
                        //_context.Pianos.Add(piano);
                        _repo.Save();
                        //_context.SaveChanges();
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
                //_context.Pianos.Where(p => p.PianoId == pianoId).SingleOrDefault(),
                Services = model.Services.ToString(),
                CustomerNotes = model.CustomerNotes,
                ServiceStart = model.ServiceStart,
                ServiceEnd = model.ServiceEnd,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                EstimatedCost = model.EstimatedCost
            };
            _repo.Appointment.Create(appointment);
            //_context.Appointments.Add(appointment);
            _repo.PendingAppointment.Delete(_repo.PendingAppointment.FindByCondition(pa => pa.PendingAppointmentId == model.PendingAppointmentId).SingleOrDefault());
            //_context.PendingAppointments.Remove(_context.PendingAppointments.Where(pa => pa.PendingAppointmentId == model.PendingAppointmentId).SingleOrDefault());
            _repo.Save();
            //_context.SaveChanges();
            _mailKitService.SendAppointmentConfirmEmail(_repo.Appointment.FindByCondition(a => a.AppointmentId == appointment.AppointmentId).Include(a => a.Piano.Client.Address).SingleOrDefault());
            //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => a.AppointmentId == appointment.AppointmentId).SingleOrDefault());
            return RedirectToAction("Index");
        }

        public void CheckForReminderEmails()
        {
            var pianosDueForService = _repo.Piano.FindByCondition(p => p.RemindForService <= DateTime.Today.Date).Include(p => p.Client).ToList();
            //_context.Pianos.Include(p => p.Client).Where(p => p.RemindForService <= DateTime.Today.Date).ToList();
            foreach (Piano piano in pianosDueForService)
            {
                if (!piano.Reminded)
                {
                    _mailKitService.SendServiceRemindEmail(piano);
                    piano.Reminded = true;
                    _repo.Piano.Update(piano);
                    //_context.Update(piano);
                }
            }
            _repo.Save();
            //_context.SaveChanges();
            var upcomingAppointments = _repo.Appointment.FindByCondition(a => a.ServiceStart.Date == DateTime.Today.AddDays(1).Date).Include(a => a.Piano.Client.Address).ToList();
            //_context.Appointments.Include(a => a.Piano.Client.Address).Where(a => a.ServiceStart.Date == DateTime.Today.AddDays(1).Date).ToList();
            foreach (Appointment appointment in upcomingAppointments)
            {
                _mailKitService.SendUpcomingServiceEmail(appointment);
            }
        }
    }
}