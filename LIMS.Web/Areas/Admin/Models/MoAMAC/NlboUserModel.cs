using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.MoAMAC
{
    public class NlboUserModel:BaseEntity
    {
        public Guid NlboUserId { get; set; }
        [LIMSResourceDisplayName("Admin.NlboUser.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.NlboUser.EnglishName")]
        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.NlboUser.Email")]
        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.NlboUser.IdCardNo")]
        public string IDCardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Province")]
        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.WardNo")]
        public string WardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.WardNo")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.Common.PhoneNo")]
        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.NlboUser.PanNo")]
        public string PanNo { get; set; }

        public string PhotoId { get; set; }
    }
}
