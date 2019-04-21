using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class Key : DbItem
    {
        public int KeyId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string SerialNumber { get; set; }
    }
}
