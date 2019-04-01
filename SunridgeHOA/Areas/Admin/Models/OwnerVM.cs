using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Admin.Models
{
    public class OwnerVM
    {
        public SunridgeHOA.Models.Owner Owner { get; set; }
        public Address Address { get; set; }

        public bool IsAdmin { get; set; }
    }
}
