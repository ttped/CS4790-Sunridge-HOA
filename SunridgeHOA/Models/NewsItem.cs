using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    //public class NewsItem : DbItem
    public class NewsItem
    {
        public int NewsItemId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Header { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public int Year { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }

    }
}
