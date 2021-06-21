using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
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
            Breed = new BreedReg();
            FiscalYear = new FiscalYear();
            Unit = new Unit();
        }
        public Species Species { get; set; }

        public BreedReg Breed { get; set; }

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
        public string VaccinationId { get; set; }
        public VaccinationType Vaccination { get; set; }
        public string TreatmentType { get; set; }
        public string AnimalHealthService { get; set; }
        public string Month { get; set; }

    }
}
