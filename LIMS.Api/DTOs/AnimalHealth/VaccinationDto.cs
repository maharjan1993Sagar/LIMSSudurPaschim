using LIMS.Api.Models;
using System;

namespace LIMS.Api.DTOs.AnimalHealth
{
    public class VaccinationDto : BaseApiEntityModel
    {
        public string AnimalRegistrationId { get; set; }
        public string FarmId { get; set; }
        public string VaccinationForDisease { get; set; }
        public string FiscalYearId { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string VaccinationTypeId{ get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
