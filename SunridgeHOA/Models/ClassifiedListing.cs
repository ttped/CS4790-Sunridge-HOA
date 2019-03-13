using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class ClassifiedListing : DbItem
    {
        public int ClassifiedListingId { get; set; }
        public int OwnerId { get; set; }
        public int ClassifiedCategoryId { get; set; }

        public string ItemName { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public DateTime ListingDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        //Nav properties
        public Owner Owner { get; set; }
        public ClassifiedCategory ClassifiedCategory { get; set; }


    }
}
