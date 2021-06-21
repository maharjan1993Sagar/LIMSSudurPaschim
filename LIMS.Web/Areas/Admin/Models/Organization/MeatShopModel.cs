using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class MeatShopModel:BaseEntity
    {
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string Buff { get; set; }
        public string Chicken { get; set; }
        public string Mutton { get; set; }
        public string Pork { get; set; }
        public string Fish { get; set; }
        public string Others { get; set; }
        public string YearlyBusiness { get; set; }
        public List<OtherOrganization> Organization{ get; set; }
    }
}
