using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Piano
    {
        [Key]
        public int PianoId { get; set; }

        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string Make { get; set; }
        public string Configuration { get; set; }
        public string Model { get; set; }

        [Display(Name = "Date of Last Service")]
        [DataType(DataType.Date)]
        public DateTime? LastService { get; set; }

        [Display(Name = "Technician Notes")]
        public string TechnicianNotes { get; set; }
    }
}
