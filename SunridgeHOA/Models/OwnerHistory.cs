using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class OwnerHistory
    {
        public int OwnerHistoryId { get; set; }
        public int LogId { get; set; }
        public int OwnerId { get; set; }
        public int HistoryTypeId { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string PrivacyLevel { get; set; }
        public bool IsArchive { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Navigation Properties
        public virtual Lot Lot { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual HistoryType HistoryType { get; set; }
        public virtual ICollection<File> Files { get; set; }
    }
}
