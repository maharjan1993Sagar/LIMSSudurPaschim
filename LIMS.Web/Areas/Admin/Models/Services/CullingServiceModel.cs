using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class CullingServiceModel
    {
        [LIMSResourceDisplayName("Admin.Culling.DateOfExit")]
        public string DateOfExit { get; set; }
        [LIMSResourceDisplayName("Admin.Culling.ReasonForExit")]

        public string ReasonForExit { get; set; }
        [LIMSResourceDisplayName("Admin.Culling.Comments")]

        public string Comments { get; set; }

    }
}
