using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class PendingAppointment
    {
        [Key]
        public int PendingAppointmentId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:(###)###-####}")]
        public string Phone { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public int Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Display(Name = "Piano Brand")]
        public string PianoMake { get; set; }

        [Display(Name = "Piano Type")]
        public string PianoConfiguration { get; set; }

        [Display(Name = "Choose Services")]
        public string Services { get; set; }

        [Display(Name = "Estimated Duration")]
        public int EstimatedDuration { get; set; }

        [Display(Name = "Please enter any additional notes concerning this service:")]
        public string CustomerNotes { get; set; }

        [Display(Name = "Preferred Appointment Date")]
        [DataType(DataType.Date)]
        public DateTime PreferredAppointmentDate { get; set; }

        [Display(Name = "Appointment Start")]

        public DateTime ServiceStart { get; set; }

        [Display(Name = "Appointment End")]
        public DateTime ServiceEnd { get; set; }

        [Display(Name = "Estimated Cost")]
        [DataType(DataType.Currency)]
        public int EstimatedCost { get; set; }
    }
}
