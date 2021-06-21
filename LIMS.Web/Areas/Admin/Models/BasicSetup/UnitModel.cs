using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.BasicSetup
{
    public class UnitModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.BasicSetup.UnitNameEnglish")]

        public string UnitNameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.BasicSetup.UnitNameNepali")]

        public string UnitNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.BasicSetup.UnitShortName")]
        public string UnitShortName { get; set; }
        [LIMSResourceDisplayName("Admin.BasicSetup.Description")]
        public string Description { get; set; }
    }
}
