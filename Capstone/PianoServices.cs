using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public static class PianoServices
    {
        public class Service
        {
            public string Name { get; set; }
            public int Time { get; set; }
            public int Cost { get; set; }

        }
        public static List<Service> TuningServices = new List<Service>()
        {
            new Service() { Name="Standard Tuning", Time=90, Cost=100 },
        };

        public static List<Service> RepairServices = new List<Service>()
        {
            new Service() { Name="Keys are sticking", Time=20, Cost=0 },
            new Service() { Name="Pedals not working properly", Time=20, Cost=0 },
            new Service() { Name="One-day refining and regulation", Time=480, Cost=600 },
            new Service() { Name="Action Pick-up (for full regulation)", Time=30, Cost=0 },
        };

        public static List<Service> CleaningServices = new List<Service>()
        {
            new Service() { Name="Soundboard Cleaning and Lubrication", Time=120, Cost=150 },
        };
    }
}
