using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class NGOModel:BaseEntity
    {
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string WorkingSector { get; set; }
        public string Comodity { get; set; }
        public string PlanPeriod { get; set; }
        public string BenifitedGroups { get; set; }
        public string Remarks { get; set; }
        public List<OtherOrganization> Organization { get; set; }
    }
}
