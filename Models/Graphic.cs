using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    public class Graphic
    {
        public int Id { get; set; }
        public string Text { get; set;}

        public int ChairId { get; set; }
        public Chair Chair { get; set; }
    }
}
