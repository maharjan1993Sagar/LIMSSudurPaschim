using LIMS.Domain.AInR;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Vaccination;
using System;

namespace LIMS.Domain.Services
{
    public class AnimalVaccination : BaseEntity
    {
        public AnimalRegistration AnimalRegistration { get; set; }
        public string AnimalRegistrationId { get; set; }
        public Farm Farm { get; set; }
        public string FarmId { get; set; }
        public VaccinationType VaccinationType { get; set; }
        public Disease Disease { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string AmountReceived { get; set; }
        public string ReceiptNo { get; set; }
        public string FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string Source { get; set; }
        public string VaccinationTypeId { get; set; }
        public string VaccinationSubType { get; set; }
        public string VaccinationForDisease { get; set; }
        public string UpdatedBy { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CreatedBy { get; set; }
    }
}
