using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class FeedIndustry:BaseEntity
    {
        public FeedIndustry()
        {
            this.OtherOrganization = new OtherOrganization();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear   FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string CowBuffalo { get; set; }
        public string SheepGoat { get; set; }
        public string Pig { get; set; }
        public string Chicken { get; set; }
        public string Fish { get; set; }
        public string Choker { get; set; }
        public string Others { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

    }
}
