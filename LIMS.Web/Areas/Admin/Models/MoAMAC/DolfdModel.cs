using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.MoAMAC
{
    public partial class DolfdModel:BaseEntity
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
        [LIMSResourceDisplayName("Admin.Common.Address")]

        public string AddressNepali { get; set; }
        public MoAMACModel MoAMAC { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.Molmac")]
        public string MoamacId { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.Email")]

        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.Website")]

        public string Website { get; set; }
        //user Details
        [LIMSResourceDisplayName("Admin.dolfd.UserNameNepali")]

        public string UserNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.UserNameEnglish")]

        public string UserNameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.IdCardNo")]

        public string IDCardNo { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.UserEmail")]

        public string UserEmail { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.Password")]
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
        [LIMSResourceDisplayName("Admin.dolfd.UserPhone")]
        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.UserPosition")]
        public string Position { get; set; }
        [LIMSResourceDisplayName("Admin.dolfd.Type")]
        public string Type { get; set; }

        public string CustomerId { get; set; }


    }
}
