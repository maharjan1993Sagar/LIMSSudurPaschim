using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Semen
{
    public class SemenDistributionModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.SemenDistribution.ServiceProviderId")]
        public string ServiceProviderId { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.SpeciesId")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.BreedId")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.BullId")]
        public string AnimalRegistrationId { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.SireId")]
        public string SireId { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.DamId")]
        public string DamId { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.EarTag")]
        public string EarTag { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.BullName")]
        public string BullName { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.Dose")]
        public string Dose { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.TotalAmount")]
        public string TotalAmount { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.FiscalYear")]
        public string FiscalYearId { get; set; }
       
        [LIMSResourceDisplayName("Admin.SemenDistribution.OrganizationName")]
        public string OrganizationName { get; set; }
        [LIMSResourceDisplayName("Admin.SemenDistribution.Type")]
        public string Type { get; set; }


        [LIMSResourceDisplayName("Admin.SemenDistribution.Date")]
        [UIHint("date")]
        public string Date { get; set; }


    }
}
