using LIMS.Core.ModelBinding;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class FeedIndustryModel
    {
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string CowBuffalo { get; set; }
        public string SheepGoat { get; set; }
        public string Pig { get; set; }
        public string Chicken { get; set; }
        public string Fish { get; set; }
        public string Choker { get; set; }
        public string Others { get; set; }
        public string Remarks { get; set; }
        public List<OtherOrganization> Organization { get; set; }

    }
}
