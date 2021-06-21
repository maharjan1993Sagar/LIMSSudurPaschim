using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class OtherOrganizationModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.EnglishName")]
        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Provience")]
        public string Provience { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.District")]
        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.LocalLevel")]
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Tole")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Email")]

        public string Email { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.ContactPerson")]

        public string ContactPersonName { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.MobileNo")]

        public string MobileNo { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.proprietor")]
        public string Proprietor { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.ContactNo")]
        public string ContactNo { get; set; }
        [LIMSResourceDisplayName("Admin.Organization.OtherOrganization.Website")]
        public string Website { get; set; }

      
    }
}
