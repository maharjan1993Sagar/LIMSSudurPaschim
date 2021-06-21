using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class OtherOrganizationDetailsModel:BaseEntity
    {
        public string OtherOrganizationId { get; set; }
        public string DailyCollection { get; set; }
        public string TotalCapacity { get; set; }
        public string TotalPeopleToBringMilkCount { get; set; }
        public string SellingPlace { get; set; }
        public string TotalBusiness { get; set; }
        public string PlaceForCOllection { get; set; }
        public string AnimalType { get; set; }
        public string TotalAnimals { get; set; }
        public string FemaleShareMembers { get; set; }
        public string MaleShareMembers { get; set; }
        public string Dayofthemarket { get; set; }
        public string Ownership { get; set; }
        public string TotalNoOfSoldAnimals { get; set; }
        public string TotalArea { get; set; }
        public string YearlyContractPrice { get; set; }
        public string YearlyProductionPerMT { get; set; }
        public string ParentsType { get; set; }
        public string ParentsBreed { get; set; }
        public string YearlyProducedChickens { get; set; }
        public string YearlySoldAnimals { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public List<OtherOrganization> Organization { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public string Remarks { get; set; }
        public string ResearchCenterType { get; set; }
        public string TotalLivestock { get; set; }
        public string TotalAreaForFeedAndForder { get; set; }
        public string ProducedThings { get; set; }
        public string SoldFormResearchCenter { get; set; }

        public string FodderFoundInResearchCenter { get; set; }
        public string QuantityOfProvidedgrass { get; set; }





    }
}
