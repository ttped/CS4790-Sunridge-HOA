using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
