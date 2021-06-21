using LIMS.Core.ModelBinding;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class VetClinicModel
    {
      
        public List<OtherOrganization> Organization { get; set; }
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }

        public string ProvidedServices { get; set; }
        public string MaleManPower { get; set; }
        public string FemaleManpower { get; set; }
        public string Remarks { get; set; }
     
    }
}
