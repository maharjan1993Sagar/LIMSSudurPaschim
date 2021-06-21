using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class LivestockResearchCenter:BaseEntity
    {
        public LivestockResearchCenter()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Type { get; set; }
        public string TotalNoOfLivestock { get; set; }
        public string TotalAreaForFeedAndFodder { get; set; }
        public string ManufacturedGoods { get; set; }
        public string SelledLivestockQuantity { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }

}
