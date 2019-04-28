using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class FormIndexVM
    {
        public List<FormResponse> Unresolved { get; set; }
        public List<FormResponse> Resolved { get; set; }

        public FormIndexVM()
        {
            Unresolved = new List<FormResponse>();
            Resolved = new List<FormResponse>();
        }
    }
}
