using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.MoAMAC
{
    public class NlboModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Dolfd.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.Englishname")]

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
        [LIMSResourceDisplayName("Admin.Common.Address")]

        public string Address { get; set; }
     
        [LIMSResourceDisplayName("Admin.Dolfd.Email")]

        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.Website")]

        public string Website { get; set; }
        //user Details
        [LIMSResourceDisplayName("Admin.Dolfd.UserNameNepali")]

        public string UserNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.UserNameEnglish")]

        public string UserNameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.IdCardNo")]

        public string IDCardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.UserEmail")]
        [UIHint("Email")]
        public string UserEmail { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Provience")]
        public string UserProvince { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]
        public string UserDistrict { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string UserLocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]
        public string UserWard { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Tole")]

        public string UserTole { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.UserPhone")]
        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.Dolfd.UserPosition")]
        public string Position { get; set; }
        [LIMSResourceDisplayName("Admin.Nlbo.MoaldId")]
        public string MOALDId { get; set; }
        public string CustomerId { get; set; }

    }
}
