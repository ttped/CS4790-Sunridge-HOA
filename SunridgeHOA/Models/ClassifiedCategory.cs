using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class ClassifiedCategory : DbItem
    {
        public int ClassifiedCategoryId { get; set; }
        [Display(Name = "Category")]
        public string Description { get; set; }

    }
}
