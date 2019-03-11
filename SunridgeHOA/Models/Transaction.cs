using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Transaction : DbItem
    {
        public int TransactionId { get; set; }
        public int LotId { get; set; }
        public int OwnerId { get; set; }
        public int TransactionTypeId { get; set; }

        public string Description { get; set; }
        public float Amount { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DatePaid { get; set; }
        public string Status { get; set; }

        //Nav properties
        public Lot Lot { get; set; }
        public Owner Owner { get; set; }
        public TransactionType TransactionType { get; set; }

    }
}
