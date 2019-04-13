using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class DocumentVM
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Document Type")]
        public int HistoryType { get; set; }

        [Display(Name = "Admin Only?")]
        public bool AdminOnly { get; set; }
    }
}
