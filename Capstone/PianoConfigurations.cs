using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public static class PianoConfigurations
    {
        public static List<SelectListItem> Configurations = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Grand", Value="Grand" },
            new SelectListItem() { Text="Upright", Value="Upright" },
        };
    }
}
