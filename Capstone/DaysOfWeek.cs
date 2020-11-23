using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public static class DaysOfWeek
    {
        public class Day
        {
            public string Name { get; set; }
            public DayOfWeek Value { get; set; }
        }

        public static List<Day> Days = new List<Day>()
        {
            new Day() { Name="Sunday", Value=DayOfWeek.Sunday },
            new Day() { Name="Monday", Value=DayOfWeek.Monday },
            new Day() { Name="Tuesday", Value=DayOfWeek.Tuesday },
            new Day() { Name="Wednesday", Value=DayOfWeek.Wednesday },
            new Day() { Name="Thursday", Value=DayOfWeek.Thursday },
            new Day() { Name="Friday", Value=DayOfWeek.Friday },
            new Day() { Name="Saturday", Value=DayOfWeek.Saturday },
        };
    }
}
