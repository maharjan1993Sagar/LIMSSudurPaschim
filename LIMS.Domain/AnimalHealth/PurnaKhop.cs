using LIMS.Domain.AInR;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AnimalHealth
{
    public class PurnaKhop:BaseEntity
    {
        public FarmForPurnaKhop FarmForPurnaKhop { get; set; }
        public string FarmForPurnaKhopId { get; set; }
        public string FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public VaccinationType Vaccination { get; set; }
        public string VaccinationTypeId { get; set; }
        public string VaccinationDate { get; set; }
        public string NextVaccinationDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
