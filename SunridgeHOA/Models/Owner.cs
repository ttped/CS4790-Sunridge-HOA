using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Owner
    {
        public int OwnerId { get; set; }
        public int AddressId { get; set; }
        public int? CoOwnerId { get; set; }

        public bool IsPrimary { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Occupation { get; set; }
        public DateTime Birthday { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public bool IsArchive { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Navigation properties
        public virtual Address Address { get; set; }
        [ForeignKey("CoOwnerId")]
        public virtual Owner CoOwner { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<OwnerHistory> OwnerHistories { get; set; }
        public virtual ICollection<ClassifiedListing> ClassifiedListings { get; set; }
        public virtual ICollection<OwnerContactType> OwnerContactTypes { get; set; }
        public virtual ICollection<KeyHistory> KeyHistories { get; set; }

    }
}
