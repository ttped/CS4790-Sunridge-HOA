using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Admin.Models
{
    public class BatchOwnerCreateVM
    {
        public List<MinOwnerInfo> OwnerList { get; set; }
        public MinOwnerInfo PrimaryOwner { get; set; }
        public Address Address { get; set; }
        public int LotId { get; set; }

        public BatchOwnerCreateVM()
        {
            OwnerList = new List<MinOwnerInfo>();
        }
    }
}
