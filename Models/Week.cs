using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    [BindProperties(SupportsGet = false)]
    public class Week
    {
        public int Id { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public byte NumberWeek { get; set; }
        public byte Course { get; set; }
        public string Group { get; set; }

        [BindProperty]
        [BindRequired]
        public List<ScheduleDay> ScheduleDays { get; set; }

        public Week()
        {
            ScheduleDays = new List<ScheduleDay>();
        }
    }
}
