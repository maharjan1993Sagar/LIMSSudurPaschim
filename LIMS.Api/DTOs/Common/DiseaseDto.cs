using LIMS.Domain;
using System;

namespace LIMS.Api.DTOs.Common
{
    public class DiseaseDto:BaseEntity
    {
        public Guid DiseaseId { get; set; }
        public string DiseaseNameNepali { get; set; }
        public string DiseaseNameEnglish { get; set; }
        public string Symptoms { get; set; }
    }
}
