using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class FishSrotModel:BaseEntity
    {
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string NoOfPond { get; set; }
        public string ReaervoirArea { get; set; }
        public string FishBreed { get; set; }
        public string Hasling { get; set; }
        public string Fry { get; set; }
        public string Fingerling { get; set; }
        public List<OtherOrganization> Organization { get; set; }
    }
}
