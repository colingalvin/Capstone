using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class DefaultTime
    {
        [Key]
        public int DefaultTimeId { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [ForeignKey("RuleSet")]
        public int RuleSetId { get; set; }
        public RuleSet RuleSet { get; set; }
    }
}
