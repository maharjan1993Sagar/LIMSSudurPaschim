using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class MeatShop:BaseEntity
    {
        public MeatShop()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Buff { get; set; }
        public string Chicken { get; set; }
        public string Mutton { get; set; }
        public string Pork { get; set; }
        public string Fish { get; set; }
        public string Others { get; set; }
        public string YearlyBusiness { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
