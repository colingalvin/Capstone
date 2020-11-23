using Capstone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModels
{
    public class CreateNewRuleSetViewModel
    {
        public int RuleSetId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool Default { get; set; }
        public List<DayOfWeek> Days { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        public List<DateTime> DefaultTimes { get; set; }
        public string StreetAddress { get; set; }
        public int Zip { get; set; }
    }
}
