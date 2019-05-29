using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class File : DbItem
    {
        public int FileId { get; set; }
        public int LotHistoryId { get; set; }

        public string FileURL { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        //Nav properties
        public LotHistory LotHistory { get; set; }
    }
}
