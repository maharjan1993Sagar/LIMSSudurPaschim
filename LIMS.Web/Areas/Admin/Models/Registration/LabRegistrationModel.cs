using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Registration
{
    public class LabRegistrationModel
    {
        [LIMSResourceDisplayName("Admin.Lab.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Lab.Englishname")]

        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Provience")]
        public string Provience { get; set; }

        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }

        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Ward")]
        public string Ward { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Tole")]
        public string Tole { get; set; }
       
        [LIMSResourceDisplayName("Admin.vhlsec.Email")]

        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.Website")]

        public string Website { get; set; }
        //user Details

    }
}
