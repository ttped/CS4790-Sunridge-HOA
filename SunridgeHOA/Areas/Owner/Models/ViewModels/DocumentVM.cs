using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class DocumentVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int HistoryType { get; set; }
        public bool AdminOnly { get; set; }
    }
}
