using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
   public  class OtherOrganizationDetails:BaseEntity
    {
        public OtherOrganizationDetails()
        {
            this.OtherOrganization = new OtherOrganization();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public string DailyCollection { get; set; }
        public string TotalCapacity { get; set; }
        public string TotalPeopleToBringMilkCount { get; set; }
        public string SellingPlace { get; set; }
        public string TotalBusiness { get; set; }
        public string PlaceForCOllection { get; set; }
        public string AnimalType { get; set; }
        public string TotalAnimals { get; set; }
        public string YearlySoldAnimals { get; set; }
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
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string Remarks { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }


    }
}
