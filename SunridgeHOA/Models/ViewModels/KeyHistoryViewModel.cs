using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models.ViewModels
{
    public class KeyHistoryViewModel
    {
        public KeyHistory KeyHistory { get; set; }
        public IEnumerable<Key> Key { get; set; }
        public IEnumerable<Lot> Lot { get; set; }
    }
}
