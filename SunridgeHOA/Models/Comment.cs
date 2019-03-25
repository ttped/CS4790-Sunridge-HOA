using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int? LotHistoryId { get; set; }
        public int? OwnerHistoryId { get; set; }
        public int OwnerId { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool Private { get; set; }

        // Nav properties
        public LotHistory LotHistory { get; set; }
        public OwnerHistory OwnerHistory { get; set; }
        public Owner Owner { get; set; }

    }
}
