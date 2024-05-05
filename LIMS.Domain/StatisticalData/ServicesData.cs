using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using LIMS.Domain.Services;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Domain.StatisticalData
{
    public class ServicesData:BaseEntity
    {
        public ServicesData()
        {
            Species = new Species();
            LivestockSpecies = new LivestockSpecies();
            Breed = new LivestockBreed();
            FiscalYear = new FiscalYear();
            Unit = new Unit();
        }
        public Species Species { get; set; }
        public LivestockSpecies LivestockSpecies { get; set; }
        public string Native { get; set; }
        public string CrossBreed { get; set; }
        public string PureExotic { get; set; }
        public LivestockBreed Breed { get; set; }
        public string ServicesType { get; set; }
        public string Quantity { get; set; }
        public Unit Unit { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string Provience { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public string Date { get; set; }
        public string Quater { get; set; }
        public string CreatedBy { get; set; }
        public Farm Farm { get; set; }
        public string FarmId { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public string TechnicianName { get; set; }
        public string VaccinationId { get; set; }
        public VaccinationType Vaccination { get; set; }
        public string TreatmentType { get; set; }
        public string AnimalHealthService { get; set; }
        public string Month { get; set; }
        public string FirmRegistrationServiceType { get; set; }
        public string BreedType { get; set; }
    }
}
