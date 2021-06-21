using LIMS.Core.ModelBinding;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class FeedFodderResearchCenterModel
    {
       
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string FeedFodderFound { get; set; }
        public string AreaForFeedAndFodder { get; set; }
        public string QuantityForSeeds { get; set; }
        public string WomenManpower { get; set; }
        public string MaleManPower { get; set; }
        public string Remarks { get; set; }
        public List<OtherOrganization> Organization { get; set; }
    }
}
