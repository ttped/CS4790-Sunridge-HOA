using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class OwnerContactType : DbItem
    {
        public int OwnerContactTypeId { get; set; }
        public int OwnerId { get; set; }
        public int ContactTypeId { get; set; }

        public string ContactValue { get; set; }

        //Nav Properties
        public Owner Owner { get; set; }
        public ContactType ContactType { get; set; }

    }
}
