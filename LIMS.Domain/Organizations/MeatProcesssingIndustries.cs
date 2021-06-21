using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class MeatProcesssingIndustries:BaseEntity
    {
        public MeatProcesssingIndustries()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string DailyProcessingCapacity { get; set; }
        public string ChickenSusage { get; set; }
        public string PorkSusage { get; set; }
        public string SukutiBuff { get; set; }
        public string BuffSusage { get; set; }
        public string Fish { get; set; }
        public string ProcessedMeat { get; set; }
        public string Others { get; set; }
        public string Market { get; set; }
        public string FemaleManpower { get; set; }
        public string MaleManpower { get; set; }
        public string YearlyBusiness { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
