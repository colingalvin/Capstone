using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class RuleSet
    {
        [Key]
        public int RuleSetId { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Default")]
        public bool Default { get; set; }

        [Display(Name = "Home Location:")]
        [ForeignKey("Address")]
        public int HomeAddressId { get; set; }
        public Address Address { get; set; }
    }
}
