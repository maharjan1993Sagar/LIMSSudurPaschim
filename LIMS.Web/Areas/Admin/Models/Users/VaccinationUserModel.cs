using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Users
{
    public class VaccinationUserModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.VaccinationUser.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.EnglishName")]
        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.Email")]
        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.IdCardNo")]
        public string IDCardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Province")]
        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.WardNo")]
        public string WardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Tole")]
        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.PhoneNo")]
        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.AccadamicQualification")]
        public string AccadamicQualification { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationUser.PanNo")]
        public string PanNo { get; set; }
      

    }
}
