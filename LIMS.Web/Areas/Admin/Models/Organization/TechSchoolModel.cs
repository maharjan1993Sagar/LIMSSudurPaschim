using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class TechSchoolModel:BaseEntity
    {
        public List<OtherOrganization> Organization { get; set; }
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }

        public string SubjectToBeTaught { get; set; }
        public string Duration { get; set; }
        public string CoordinatingBody { get; set; }
        public string Remarks { get; set; }
    }
}
