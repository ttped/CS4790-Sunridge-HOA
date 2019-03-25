using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Owner
    {
        public int OwnerId { get; set; }
        public int AddressId { get; set; }
        //public int? CoOwnerId { get; set; }
 

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Occupation { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Emergency Contact")]
        public string EmergencyContactName { get; set; }

        [Required]
        [Display(Name = "Emergency Contact #")]
        public string EmergencyContactPhone { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchive { get; set; } = false;

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Navigation properties
        public virtual Address Address { get; set; }
        public virtual ICollection<OwnerLot> OwnerLots { get; set; }

        //[Display(Name = "Co-owner")]
        //[ForeignKey("CoOwnerId")]
        //public virtual Owner CoOwner { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<OwnerHistory> OwnerHistories { get; set; }
        public virtual ICollection<ClassifiedListing> ClassifiedListings { get; set; }
        public virtual ICollection<OwnerContactType> OwnerContactTypes { get; set; }
        public virtual ICollection<KeyHistory> KeyHistories { get; set; }

        // Calculated properties
        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

    }
}
