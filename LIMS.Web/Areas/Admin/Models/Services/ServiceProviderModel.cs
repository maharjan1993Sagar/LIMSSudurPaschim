using LIMS.Core.ModelBinding;
using LIMS.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class ServiceProviderModel: BaseEntity
    {
        [LIMSResourceDisplayName("Admin.ServiceProvider.NameNepali")]

        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.NameEnglish")]

        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.Phone")]

        public string MobileNo { get; set; }
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
        [LIMSResourceDisplayName("Admin.Common.Latitude")]

        public string Latitude { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Longitude")]

        public string Longitude { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.ProfessionalType")]

        public string ProfessionalType { get; set; }
         [LIMSResourceDisplayName("Admin.ServiceProvider.EmployeeId")]

        public string EmployeeId { get; set; }

        [LIMSResourceDisplayName("Admin.ServiceProvider.Designation")]

        public string Designation { get; set; }
      
        [LIMSResourceDisplayName("Admin.ServiceProvider.Email")]
        [UIHint("Email")]
        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.Password")]
        [UIHint("Password")]
        public string Password { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.CitnshipNo")]

        public string CitizenshipNo { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.PanNo")]

        public string PanNo{ get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.ServiceProviderType")]

        public string ServiceProviderType { get; set; }

        [LIMSResourceDisplayName("Admin.ServiceProvider.MunicipalityTaxIdentificationNumber")]

        public string MunicipalityTaxIdentificationNumber { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.IsPprs")]

        public bool IsPprs { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.Date")]
        [UIHint("date")]
        public DateTime StartDate { get; set; }
        [LIMSResourceDisplayName("Admin.ServiceProvider.IsActive")]
        public bool IsActive { get; set; }
        public string CustomerId { get; set; }
    }
}
