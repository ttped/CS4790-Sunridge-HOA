using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class HistoryType
    {
        public int HistoryTypeId { get; set; }

        public string Description { get; set; }
        public bool IsArchive { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Nav properties
        public virtual ICollection<LotHistory> LotHistories { get; set; }

    }
}
