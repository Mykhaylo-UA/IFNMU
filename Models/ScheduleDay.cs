using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFNMUSiteCore.Models
{
    public class ScheduleDay
    {
        public int Id { get; set; }
        public byte DayNumber { get; set; }

        [BindProperty]
        [FromForm]
        public List<Lesson> Lessons { get; set; }

        public int WeekId { get; set; }
        public Week Week { get; set; }

        public ScheduleDay()
        {
            Lessons = new List<Lesson>();
        }
    }
}