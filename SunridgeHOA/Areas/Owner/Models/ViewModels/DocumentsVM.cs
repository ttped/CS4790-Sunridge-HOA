using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class DocumentsVM
    {
        public List<SunridgeHOA.Models.File> Files { get; set; }
    }
}
