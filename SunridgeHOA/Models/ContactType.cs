using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class ContactType : DbItem
    {
        public int ContactTypeId { get; set; }
        public string Value { get; set; }

    }
}
