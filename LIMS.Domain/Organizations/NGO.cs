using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class NGO:BaseEntity
    {
        public NGO()
        {
            this.OtherOrganization = new OtherOrganization();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string WorkingSector { get; set; }
        public string Comodity { get; set; }
        public string PlanPeriod { get; set; }
        public string BenifitedGroups { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
