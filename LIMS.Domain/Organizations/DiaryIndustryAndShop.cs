using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class DiaryIndustryAndShop:BaseEntity
    {
        public DiaryIndustryAndShop()
        {
            this.OtherOrganization = new OtherOrganization();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string TotalMilkConsumption { get; set; }
        public string Milk { get; set; }
        public string Curd { get; set; }
        public string Ghew { get; set; }
        public string Panir { get; set; }
        public string Cheese { get; set; }
        public string Churpi { get; set; }
        public string IceCream { get; set; }
        public string Mithai { get; set; }
        public string Others { get; set; }
        public string MaleManPower { get; set; }
        public string FemaleManPower { get; set; }
        public string YearlyBusinessIncome { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
