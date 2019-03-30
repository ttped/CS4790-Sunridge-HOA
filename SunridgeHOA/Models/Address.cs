using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Display(Name = "Street Address")]
        [Required]
        public string StreetAddress { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool IsArchive { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public override string ToString()
        {
            return String.Join(' ', StreetAddress, City, State, Zip);
        }
    }
}