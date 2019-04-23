using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class KeyHistory: DbItem
    {
        public int KeyHistoryId { get; set; }
        public int KeyId { get; set; }
        public int OwnerId { get; set; }

        public string Status { get; set; }
        [Display(Name="Date Issued")]
        public DateTime DateIssued { get; set; }
        public DateTime DateReturned { get; set; }
        public float PaidAmount { get; set; }


        //Nav properties
        public Key Key { get; set; }
        public Owner Owner { get; set; }

    }
}
