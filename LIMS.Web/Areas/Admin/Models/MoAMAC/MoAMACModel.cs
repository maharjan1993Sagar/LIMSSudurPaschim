using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.MoAMAC
{
    public class MoAMACModel:BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Moamac.EnglishName")]

        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Moamac.NepaliName")]

        public string NameNepali { get; set; }
    }
}
