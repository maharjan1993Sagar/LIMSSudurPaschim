using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class FeedFodderResearchCenter: BaseEntity
    {
        public FeedFodderResearchCenter()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string FeedFodderFound { get; set; }
        public string AreaForFeedAndFodder { get; set; }
        public  string QuantityForSeeds { get; set; }
        public string WomenManpower { get; set; }
        public string MaleManPower { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
