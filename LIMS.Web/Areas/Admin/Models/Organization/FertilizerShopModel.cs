using LIMS.Core.ModelBinding;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class FertilizerShopModel
    {
      
        public List<OtherOrganization> Organization { get; set; }
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }

        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public decimal Fertilizer1 { get; set; }
        public decimal Fertilizer2 { get; set; }
        public decimal Fertilizer3 { get; set; }
        public decimal FertilizerOther { get; set; }
        public string Remarks { get; set; }

    }
}
