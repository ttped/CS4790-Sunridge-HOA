using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SunridgeHOA.Models
{
    public class KeyHistory: DbItem
    {
        public int KeyHistoryId { get; set; }
        public int KeyId { get; set; }

        [Display(Name = "Lot")]
        public int LotId { get; set; }

        public string Status { get; set; }
        [Display(Name ="Date Issued")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateIssued { get; set; }
        [Display(Name = "Date Returned")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateReturned { get; set; }
        [Display(Name = "Paid Amount")]
        public float PaidAmount { get; set; }


        //Nav properties
        public Key Key { get; set; }
        public Lot Lot { get; set; }

    }
}
