using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public static class DaysOfWeek
    {
        public static List<SelectListItem> Days = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Sunday", Value="Sunday" },
            new SelectListItem() { Text="Monday", Value="Monday" },
            new SelectListItem() { Text="Tuesday", Value="Tuesday" },
            new SelectListItem() { Text="Wednesday", Value="Wednesday" },
            new SelectListItem() { Text="Thursday", Value="Thursday" },
            new SelectListItem() { Text="Friday", Value="Friday" },
            new SelectListItem() { Text="Saturday", Value="Saturday" },
        };
    }
}
