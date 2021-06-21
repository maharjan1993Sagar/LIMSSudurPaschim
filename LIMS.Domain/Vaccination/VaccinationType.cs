using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Vaccination
{
   public class VaccinationType:BaseEntity
    {
        public VaccinationType()
        {
            this.VaccinationTypeGuid = Guid.NewGuid();
        }
        public Guid VaccinationTypeGuid { get; set; }
        public string CommonName { get; set; }
        public string MedicalName { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string CreatedBy { get; set; }
        public List<string> Species { get; set; }
    }
}
