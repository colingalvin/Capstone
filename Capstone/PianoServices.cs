using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public static class PianoServices
    {
        public static List<SelectListItem> Services = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Standard Tuning", Value="90" },
            new SelectListItem() { Text="Pitch Raise", Value="60" },
            new SelectListItem() { Text="Pedals Sticking", Value="20" },
        };
    }
}
