using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ChairId { get; set; }
        public Chair Chair { get; set; }

        public int? FolderId { get; set; }
        [ForeignKey("FolderId")]
        public Folder FolderUp { get; set; }

        public List<File> Files { get; set; }
        public List<Folder> Folders { get; set; }

        public Folder()
        {
            Files = new List<File>();
            Folders = new List<Folder>();
        }
    }
}
