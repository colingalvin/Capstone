using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public static class AppointmentTimes
    {
        public static List<SelectListItem> Times = new List<SelectListItem>()
        {
            new SelectListItem() { Text="7:00am", Value="7:00am" },
            new SelectListItem() { Text="8:00am", Value="8:00am" },
            new SelectListItem() { Text="9:00am", Value="9:00am" },
            new SelectListItem() { Text="10:00am", Value="10:00am" },
            new SelectListItem() { Text="11:00am", Value="11:00am" },
            new SelectListItem() { Text="12:00pm", Value="12:00pm" },
            new SelectListItem() { Text="1:00pm", Value="1:00pm" },
            new SelectListItem() { Text="2:00pm", Value="2:00pm" },
            new SelectListItem() { Text="3:00pm", Value="3:00pm" },
            new SelectListItem() { Text="4:00pm", Value="4:00pm" },
            new SelectListItem() { Text="5:00pm", Value="5:00pm" },
            new SelectListItem() { Text="6:00pm", Value="6:00pm" },
            new SelectListItem() { Text="7:00pm", Value="7:00pm" },
            new SelectListItem() { Text="8:00pm", Value="8:00pm" },
        };
    }
}
