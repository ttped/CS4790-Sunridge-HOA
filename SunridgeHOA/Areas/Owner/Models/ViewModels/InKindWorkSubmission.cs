using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Areas.Owner.Models.ViewModels
{
    public class InKindWorkSubmission
    {
        public FormResponse FormResponse { get; set; }
        public List<InKindWorkHours> LaborHours { get; set; }
        public List<InKindWorkHours> EquipmentHours { get; set; }

        public InKindWorkSubmission()
        {
            LaborHours = new List<InKindWorkHours>(3);
            EquipmentHours = new List<InKindWorkHours>(3);

            for (int i = 0; i < 3; i++)
            {
                LaborHours.Add(new InKindWorkHours());
                EquipmentHours.Add(new InKindWorkHours());
            }
        }
    }
}
