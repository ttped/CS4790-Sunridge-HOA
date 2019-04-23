using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    //public class NewsItem : DbItem
    public class NewsItem
    {
        public int NewsItemId { get; set; }
        //public int FileId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        //public DateTime Year { get; set; }
        public int Year { get; set; }
        public string Image { get; set; }
        //public string Searching { get; set; }

        // Nav properties
        //public File File { get; set; }

    }
}
