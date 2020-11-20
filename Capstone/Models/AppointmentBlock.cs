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
        public string Day { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [ForeignKey("RuleSet")]
        public int RuleSetId { get; set; }
        public RuleSet RuleSet { get; set; }
    }
}
