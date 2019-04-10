﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// Courtesy of other Sunridge Team
namespace SunridgeHOA.Models
{
    public class ScheduledEvent
    {
        public int ID { get; set; }
        [Required] public string Subject { get; set; }
        public string Description { get; set; }
        [Required] public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        [Required] public bool IsFullDay { get; set; }
    }
}
