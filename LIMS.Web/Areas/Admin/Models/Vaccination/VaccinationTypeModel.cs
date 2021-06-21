using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Vaccination
{
    public class VaccinationTypeModel: BaseEntityModel
    {

        [LIMSResourceDisplayName("Admin.Vaccinnation.VaccinationType.CommonName")]
        public string CommonName { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccinnation.VaccinationType.Medicalname")]
        public string MedicalName { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccinnation.VaccinationType.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccinnation.VaccinationType.Description")]

        public string Description { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccinnation.VaccinationType.Brand")]
        public string Brand { get; set; }
        [LIMSResourceDisplayName("Admin.Vaccination.Vaccination.Specification")]

        public string Specification { get; set; }
        public List<string> Species { get; set; }

    }
}
