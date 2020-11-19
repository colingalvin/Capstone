using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class AppointmentBlock
    {
        [Key]
        public int AppointmentBlockId { get; set; }

        [Display(Name = "Day")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [ForeignKey("RuleSet")]
        public int RuleSetId { get; set; }
        public RuleSet RuleSet { get; set; }
    }
}
