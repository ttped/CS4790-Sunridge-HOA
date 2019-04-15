using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class ClassifiedImage
    {
        public int ClassifiedImageId { get; set; }
        public int ClassifiedListingId { get; set; }

        public bool IsMainImage { get; set; }
        public string ImageURL { get; set; }
        public string ImageExtension { get; set; }
    }
}
