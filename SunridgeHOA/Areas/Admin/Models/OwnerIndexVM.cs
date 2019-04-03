using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Admin.Models
{
    public class OwnerIndexVM
    {
        public SunridgeHOA.Models.Owner Owner { get; set; }
        public List<string> Lots { get; set; }
    }
}
