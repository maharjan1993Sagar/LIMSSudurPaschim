using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.MoAMAC
{
    public class VhlsecModel:BaseEntity
    {
        

        [LIMSResourceDisplayName("Admin.Vhlec.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Vhlec.Englishname")]

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
        public DolfdModel Dolfd { get; set; }
        [LIMSResourceDisplayName("Admin.Vhlsec.Dolfd")]
        public string DolfdId { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.Email")]

        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.Website")]

        public string Website { get; set; }
        //user Details
        [LIMSResourceDisplayName("Admin.vhlsec.UserNameNepali")]

        public string UserNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.UserNameEnglish")]

        public string UserNameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.IdCardNo")]

        public string IDCardNo { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.UserEmail")]

        public string UserEmail { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.Password")]
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
        [LIMSResourceDisplayName("Admin.vhlsec.UserPhone")]
        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.vhlsec.UserPosition")]
        public string Position { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
    }
}
