using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFNMUSiteCore.Models
{
    public class ThematicPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string PathOfficeOnline { get; set; }

        public ICollection<Lesson> Lessons { get; set; }

        public ThematicPlan()
        {
            Lessons = new List<Lesson>();
        }
    }
}