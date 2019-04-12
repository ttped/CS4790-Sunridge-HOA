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
        //public int ClassifiedListingId { get; set; }

        //int or bool?
        //public int IsMainImage{ get; set; }
        public string FileURL { get; set; }
        //public string ImageContentType { get; set; }
        //public string FileStream { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }


        //Nav properties
        public LotHistory LotHistory { get; set; }
        public ClassifiedListing ClassifiedListing { get; set; }
    }
}
