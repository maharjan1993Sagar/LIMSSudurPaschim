using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class FishSrot:BaseEntity
    {
        public FishSrot()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string NoOfPond { get; set; }
        public string ReaervoirArea { get; set; }
        public string FishBreed { get; set; }
        public string Hasling { get; set; }
        public string Fry { get; set; }
        public string Fingerling { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
