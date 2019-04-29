using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Admin.Models
{
    public class MinOwnerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
