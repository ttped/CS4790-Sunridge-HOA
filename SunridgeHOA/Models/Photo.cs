using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public int OwnerId { get; set; }
        public string Category { get; set; }
        //public string Writer { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Image { get; set; }


        //Nav props
        [ForeignKey("OwnerId")]
        public virtual Owner Owner { get; set; }

    }
}
