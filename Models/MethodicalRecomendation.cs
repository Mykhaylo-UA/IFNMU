using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    public class MethodicalRecomendation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string PathOfficeOnline { get; set; }

        public ICollection<Lesson> Lessons { get; set; }

        public MethodicalRecomendation()
        {
            Lessons = new List<Lesson>();
        }
    }
}
