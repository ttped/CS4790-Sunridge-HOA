using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models.ViewModels
{
    //for admin
    public class ClassifiedListingViewModel
    {
        public ClassifiedListing ClassifiedListing { get; set; }
        public IEnumerable<ClassifiedCategory> ClassifiedCategory { get; set; }
        public IEnumerable<Owner> Owner { get; set; }
        public IEnumerable<ClassifiedImage> ClassifiedImages { get; set; }
    }

}
