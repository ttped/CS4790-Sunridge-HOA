using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models.ViewModels
{
    public class AdminPhotoViewModels
    {
        public Photo Photo { get; set; }
        public IEnumerable<Owner> Owner { get; set; }
    }
}
