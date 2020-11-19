using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Data;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return View();
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
    }
}