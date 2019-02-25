using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Key : DbItem
    {
        public int KeyId { get; set; }
        public string SerialNumber { get; set; }
    }
}
