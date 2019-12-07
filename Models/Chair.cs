using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    public class Chair
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public string PathAvatar { get; set; }

        public string PathChairIFNMU { get; set; }

        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }
        public List<Advertisement> Advertisements { get; set; }
        public List<Graphic> Graphics { get; set; }

        public Chair()
        {
            Folders = new List<Folder>();
            Files = new List<File>();
            Advertisements = new List<Advertisement>();
            Graphics = new List<Graphic>();
        }
    }
}
