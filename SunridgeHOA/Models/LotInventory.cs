using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class LotInventory
    {
        public int LotInventoryId { get; set; }
        public int LotId { get; set; }
        public int InventoryId { get; set; }

        public string Description { get; set; }
        public bool IsArchive { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Nav props
        public virtual Lot Lot { get; set; }
        public virtual Inventory Inventory { get; set; }

    }
}
