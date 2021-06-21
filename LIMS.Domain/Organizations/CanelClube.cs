using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class CanelClube:BaseEntity
    {
        public CanelClube()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string DogBreedToBeSold { get; set; }
        public string ProvidedServices { get; set; }
        public string MaleManPower { get; set; }
        public string FemaleManpower { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
