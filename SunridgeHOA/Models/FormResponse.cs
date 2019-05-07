using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class FormResponse
    {
        public int FormResponseId { get; set; }
        public int OwnerId { get; set; }
    

        /*
         * SC: Suggestions and complaints
         * WIK: Work in kind
         * CL: Loss claim
         * BR: Building request
         */
        [Required]
        [StringLength(3, MinimumLength = 1)]
        public string FormType { get; set; }

        public int? LotId { get; set; } // work in kind

        [Display(Name = "Listing Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SubmitDate { get; set; }

        public string Description { get; set; } // suggestions, work in kind
        public string Suggestion { get; set; } // suggestions

        //public string EquipmentUsed { get; set; } // work in kind

        // Add an object to hold these?
        //public int? Hours { get; set; } // work in kind
        //public string Activity { get; set; } // work in kind 

        public String PrivacyLevel { get; set; }
        public bool Resolved { get; set; }
        [Display(Name = "Resolve Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ResolveDate { get; set; }

        [Display(Name = "Resolved by")]
        public string ResolveUser { get; set; }



        // Nav properties
        public Owner Owner { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual Lot Lot { get; set; }
        public virtual List<InKindWorkHours> InKindWorkHours { get; set; }

        // Calculated properties
        [Display(Name = "Form Type")]
        public string FormTypeName
        {
            get
            {
                switch (FormType)
                {
                    case "SC":
                        return "Suggestion / Complaint";
                    case "WIK":
                        return "Work in kind";
                    case "CL":
                        return "Loss claim";
                    case "BR":
                        return "Building request";
                    default:
                        return FormType;
                }
            }
        }

        public string FormAction
        {
            get
            {
                switch (FormType)
                {
                    case "SC":
                        return "SuggestionComplaint";
                    case "WIK":
                        return "InKindWork";
                    case "CL":
                        return "Loss claim";
                    case "BR":
                        return "Building request";
                    default:
                        return FormType;
                }
            }
        }
    }
}
