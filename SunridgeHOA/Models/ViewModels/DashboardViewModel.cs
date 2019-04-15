using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Owner Owner { get; set; }
        public List<OwnerLot> OwnerLots { get; set; }
        public List<LotInventory> lotInventories { get; set; }
        public List<KeyHistory> KeyHistories { get; set; }
        
    }
}
