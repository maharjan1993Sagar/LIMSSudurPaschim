using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class DiaryIndustryAndShopModel:BaseEntity
    {
      
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
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
        public List<OtherOrganization> Organization { get; set; }
    }
}
