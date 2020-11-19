﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [ForeignKey("PianoId")]
        public int PianoId { get; set; }
        public Piano Piano { get; set; }

        public string IncludedServices { get; set; }

        [Display(Name = "Customer Notes")]
        public string CustomerNotes { get; set; }

        [Display(Name = "Technician Notes")]
        public string TechnicianNotes { get; set; }

        [Display(Name = "Appointment Start")]
        public DateTime ServiceStart { get; set; }

        [Display(Name = "Appointment End")]
        public DateTime ServiceEnd { get; set; }

        [Display(Name = "Completed?")]
        public bool IsComplete { get; set; }
    }
}
