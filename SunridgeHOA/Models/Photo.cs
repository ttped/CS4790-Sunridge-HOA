using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public string Category { get; set; }
        [Required(ErrorMessage = "Please type a title")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Please type an year")]
        public int Year { get; set; }
        
        public string Image { get; set; }

        //Nav props
        [ForeignKey("OwnerId")]
        public virtual Owner Owner { get; set; }
    }
}
