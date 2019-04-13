using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class OwnerHistory : DbItem
    {
        public int OwnerHistoryId { get; set; }
        public int OwnerId { get; set; }
        public int HistoryTypeId { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        //public int? Hours { get; set; }
        public string Status { get; set; }


        // Nav properties
        public Owner Owner { get; set; }
        public HistoryType HistoryType { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public virtual ICollection<File> Files { get; set; }

    }
}
