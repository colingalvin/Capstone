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
            new SelectListItem() { Text="Standard Tuning", Value="Standard Tuning" },
            new SelectListItem() { Text="Pitch Raise", Value="Pitch Raise" },
            new SelectListItem() { Text="Keys Sticking", Value="Keys Sticking" },
        };
    }
}
