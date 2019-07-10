using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class DashboardVM
    {
        public SunridgeHOA.Models.Owner Owner { get; set; }
        public List<Lot> Lots{ get; set; }
        //public List<LotInventory> LotInventories { get; set; }
        public List<KeyHistory> KeyHistories { get; set; }
        
    }
}
