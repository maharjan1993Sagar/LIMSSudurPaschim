using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Organization
{
    public class MeatProcessingModel:BaseEntity
    {
      
        public string OtherOrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string DailyProcessingCapacity { get; set; }
        public string ChickenSusage { get; set; }
        public string PorkSusage { get; set; }
        public string SukutiBuff { get; set; }
        public string BuffSusage { get; set; }
        public string Fish { get; set; }
        public string ProcessedMeat { get; set; }
        public string Others { get; set; }
        public string Market { get; set; }
        public string FemaleManpower { get; set; }
        public string MaleManpower { get; set; }
        public string YearlyBusiness { get; set; }
        public List<OtherOrganization> Organization { get; set; }
    }
}
