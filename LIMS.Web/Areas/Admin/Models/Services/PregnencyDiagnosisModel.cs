using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class PregnencyDiagnosisModel:BaseEntity
    {
        public ServiceProviderModel ServiceProvider { get; set; }
        
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.ServiceProviderId")]

        public string ServiceProvicerId { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.ServiceType")]

        public string ServiceType { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.ServiceName")]

        public string ServiceName { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.Date")]
        [UIHint("Date")]
        public DateTime Date { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.Result")]

        public string Result { get; set; }
        [LIMSResourceDisplayName("Admin.common.Technician")]
        public string Technician { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Fiscalyear")]

        public string FiscalYearId { get; set; }
      

        [LIMSResourceDisplayName("Admin.Common.NaturalService")]

        public string NaturalService { get; set; }
        [LIMSResourceDisplayName("Admin.Common.OtherServiceProviced")]

        public string OtherServiceProviced { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.Reason")]

        public string Reason { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.FarmName")]
        public string FarmName { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.SpeciesName")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.BreedName")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.AnimalName")]
        public string AnimalName { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyDignosis.Eartag")]
        public string Eartag { get; set; }
       
        public string AnimalId { get; set; }

        [LIMSResourceDisplayName("Admin.PregnencyDignosis.MobileNo")]
        public string MobileNo { get; set; }

        public string FarmId { get; set; }
        public string Source { get; set; }
    }
}

