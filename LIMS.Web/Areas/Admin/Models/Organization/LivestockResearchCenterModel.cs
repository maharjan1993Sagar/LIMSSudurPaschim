using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class LivestockResearchCenterModel:BaseEntity
    {
     
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string Type { get; set; }
        public string TotalNoOfLivestock { get; set; }
        public string TotalAreaForFeedAndFodder { get; set; }
        public string ManufacturedGoods { get; set; }
        public string SelledLivestockQuantity { get; set; }
        public string Remarks { get; set; }
        public List<OtherOrganization> Organization { get; set; }
    }
}
