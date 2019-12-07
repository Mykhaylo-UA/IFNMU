using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public TypeFile TypeFile { get; set; }

        public int? FolderId { get; set; }
        public Folder Folder { get; set; }

        public int? ChairId { get; set; }
        public Chair Chair { get; set; }
    }

    public enum TypeFile
    {
        Word,
        Excel,
        PowerPoint,
        PDF,
        DJVU,
        Image,
        None
    }
}
