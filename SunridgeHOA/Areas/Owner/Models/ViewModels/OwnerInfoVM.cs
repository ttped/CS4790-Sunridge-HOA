using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class OwnerInfoVM
    {
        public SunridgeHOA.Models.Owner Owner { get; set; }
        public Address Address { get; set; }
    }
}
