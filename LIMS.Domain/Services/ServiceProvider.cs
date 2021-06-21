using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Services
{
    public class ServiceProvider:BaseEntity
    {

        public string NameNepali { get; set; }

        public string NameEnglish { get; set; }

        public string MobileNo { get; set; }

        public string Provience { get; set; }

        public string District { get; set; }

        public string LocalLevel { get; set; }

        public string Ward { get; set; }

        public string Tole { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string ProfessionalType { get; set; }

        public string EmployeeId { get; set; }


        public string Designation { get; set; }

        public string Email { get; set; }

        public string CitizenshipNo { get; set; }

        public string PanNo { get; set; }

        public string ServiceProviderType { get; set; }

        public string MunicipalityTaxIdentificationNumber { get; set; }

        public bool IsPprs { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

    }
}
