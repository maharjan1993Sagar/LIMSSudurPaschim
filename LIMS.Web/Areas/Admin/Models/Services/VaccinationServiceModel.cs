using LIMS.Core.ModelBinding;
using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.AInR;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class VaccinationServiceModel
    {
        public AnimalRegistrationModel AnimalRegistration { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationService.TechnicianName")]
        public ServiceProviderModel ServiceProviderId { get; set; }

        public ServiceProviderModel ServiceProvider { get; set; }
        public VaccinationTypeModel Vaccine { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationService.VaccinationDate")]
        [UIHint("Date")]
        public DateTime VaccinationDate { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationService.AmountReceived")]

        public string AmountReceived { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinationService.ReceiptNo")]

        public string ReceiptNo { get; set; }
      
        [LIMSResourceDisplayName("Admin.Common.Fiscalyear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.VaccinationType")]
        public string VaccinationTypeId { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.VaccinationSubType")]
        public string VaccinationSubType { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.VaccinationForDisease")]
        public string VaccinationForDisease { get; set; }
      

        [LIMSResourceDisplayName("Admin.Vaccination.FarmName")]
        public string FarmName { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.AnimalName")]
        public string AnimalName { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.Eartag")]
        public string Eartag { get; set; }

        public string AnimalId { get; set; }

        [LIMSResourceDisplayName("Admin.Vaccination.MobileNo")]
        public string MobileNo { get; set; }

        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.Species")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.Breed")]
        public string BreedId { get; set; }


    }
}
