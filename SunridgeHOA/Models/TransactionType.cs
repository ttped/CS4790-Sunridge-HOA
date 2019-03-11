using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class TransactionType : DbItem
    {
        public int Id { get; set; }
        public string Description { get; set; }

    }
}
