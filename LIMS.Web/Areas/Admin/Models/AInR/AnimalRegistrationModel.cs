using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.Livestock;
using LIMS.Web.Areas.Admin.Models.Recording;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.AInR
{
   public class AnimalRegistrationModel:BaseEntity
    {
      
        [LIMSResourceDisplayName("Admin.AnimalRegistration.SpeciesId")]

        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.BreedId")]

        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.Name")]

        public string Name { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.FarmId")]

        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.EarTagNo")]

        public string EarTagNo { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.Gender")]

        public string Gender { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.Age")]

        public int? Age { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.SireId")]

        public string SireId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.DamId")]

        public string DamId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.Weight")]

        public int? Weight { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.NoOfCalving")]

        public string NoOFCalving { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.PregencyStatus")]

        public string PregencyStatus { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.MilkStatus")]

        public string MilkStatus { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.PhysicalDefact")]

        public string PhysicalDefact { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.AnimalColor")]

        public string AnimalColor { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.DOB")]

        public DateTime? DOB { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.EntryType")]

        public string EntryType { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalRegistration.BreedType")]

        public string BreedType { get; set; }

        public FarmModel FarmModel { get; set; }


        public VaccinationServiceModel Vaccination { get; set; }
        public AIServiceModel AIService { get; set; }
        public PregnencyDiagnosisModel PregnencyDiagnosis { get; set; }
        public PregnencyTerminationModel PregnencyTermination { get; set; }
        public CalvingModel CalvingModel { get; set; }

        public HeatRecordingModel HeatRecordimg { get; set; }
        public MilkRecordingModel MilkRecordingModel { get; set; }
        public GrowthMonitoringModel GetGrowthMonitoringModel { get; set; }

        public SampleModel SampleModel { get; set; }
        public TreatmentServiceModel TreatmentService { get; set; }
        public EarTagChange EarTagChange { get; set; }
        public AnimalMovement AnimalMovement { get; set; }
        public NsModel Ns { get; set; }
        public CullingServiceModel CullingService { get; set; }
        public AnimalListModel AnimalListModel { get; set; }
    }
}
