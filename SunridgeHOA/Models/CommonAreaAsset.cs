using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class CommonAreaAsset : DbItem
    {
        public int CommonAreaAssetId { get; set; }

        public string AssetName { get; set; }
        public float PurchasePrice { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

    }
}
