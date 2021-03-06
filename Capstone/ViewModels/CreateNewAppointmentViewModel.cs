﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModels
{
    public class CreateNewAppointmentViewModel
    {
        // Track pending appointment id
        public int PendingAppointmentId { get; set; }

        // Necessary Piano Details
        public int PianoId { get; set; }
        public string Make { get; set; }
        public string Configuration { get; set; }
        
        // Necessary Client Details
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Necessary Address Details
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Necessary Appointment Details
        public string Services { get; set; }
        public string CustomerNotes { get; set; }
        public DateTime ServiceStart { get; set; }
        public DateTime ServiceEnd { get; set; }
        public int EstimatedCost { get; set; }
    }
}
