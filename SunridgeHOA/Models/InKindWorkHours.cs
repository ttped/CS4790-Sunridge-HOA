using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class InKindWorkHours
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double? Hours { get; set; }
        public string Type { get; set; }
        public int FormResponseId { get; set; }
    }
}
