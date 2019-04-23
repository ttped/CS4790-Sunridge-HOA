using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Admin.Models
{
    public class CommonAreaAssetVM
    {
        public SunridgeHOA.Models.CommonAreaAsset CommonAreaAsset { get; set; }
        public List<SunridgeHOA.Models.Maintenance> Maintenances { get; set; }
    }
}
