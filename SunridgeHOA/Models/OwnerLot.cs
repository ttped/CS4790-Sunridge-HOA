using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class OwnerLot : DbItem
    {
        public int OwnerLotId { get; set; }
        public int OwnerId { get; set; }
        public int LotId { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Nav properties
        public Owner Owner { get; set; }
        public Lot Lot { get; set; }

    }
}
