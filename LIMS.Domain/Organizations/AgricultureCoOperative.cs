using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public class AgricultureCoOperative:BaseEntity
    {
        public AgricultureCoOperative()
        {
            this.OtherOrganization = new OtherOrganization();
            this.FiscalYear = new FiscalYear();
        }
        public OtherOrganization OtherOrganization { get; set; }
        public string OtherOrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public int MaleMembers { get; set; }
        public int FemaleMembers { get; set; }
        public int OthersMembers { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
