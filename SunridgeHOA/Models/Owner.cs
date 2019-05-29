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

        public string ApplicationUserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[None listed]")]
        public string Occupation { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}",
            ConvertEmptyStringToNull = true, NullDisplayText = "[None listed]")]
        public DateTime? Birthday { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[None listed]")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[None listed]")]
        public string Email { get; set; }

        [Display(Name = "Emergency Contact")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[None listed]")]
        public string EmergencyContactName { get; set; }

        [Display(Name = "Emergency Contact #")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[None listed]")]
        public string EmergencyContactPhone { get; set; }

        [Display(Name = "Receive Sunridge emails")]
        public bool? ReceiveEmails { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchive { get; set; } = false;

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Navigation properties
        public virtual Address Address { get; set; }
        public virtual ICollection<OwnerLot> OwnerLots { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        //public virtual ICollection<OwnerHistory> OwnerHistories { get; set; }
        public virtual ICollection<FormResponse> FormResponses { get; set; }
        public virtual ICollection<ClassifiedListing> ClassifiedListings { get; set; }
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
