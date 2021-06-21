using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.AInR
{
    public class AnimalMovement
    {
        [LIMSResourceDisplayName("Admin.Movement.MovementDate")]
        public string MovementDate { get; set; }
        [LIMSResourceDisplayName("Admin.Movement.MovementType")]
        public string MovementType { get; set; }
        [LIMSResourceDisplayName("Admin.Movement.ToFarm")]
        public string ToFarm { get; set; }
    }
}
