using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models
{
    public class LotIndexVM
    {
        public Lot Lot { get; set; }
        public Address Address { get; set; }

        [Display(Name = "Primary Owner")]
        public SunridgeHOA.Models.Owner PrimaryOwner { get; set; }
        public List<SunridgeHOA.Models.Owner> Owners { get; set; }

        [Display(Name = "Lot Inventory")]
        public List<Inventory> InventoryItems { get; set; }
    }
}
