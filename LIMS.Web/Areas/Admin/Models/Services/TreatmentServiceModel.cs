using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class TreatmentServiceModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Treatment.DiseaseName")]
        public string DiseaseName { get; set; }
        [LIMSResourceDisplayName("Admin.Treatment.Symptoms")]
        public string Symptoms { get; set; }
        [LIMSResourceDisplayName("Admin.Treatment.Medicine")]
        public string Medicine { get; set; }
        [LIMSResourceDisplayName("Admin.Treatment.Date")]
        public DateTime? Date { get; set; }
        [LIMSResourceDisplayName("Admin.Treatment.Description")]
        public string Description { get; set; }
        [LIMSResourceDisplayName("Admin.Treatment.TechnicianName")]
        public string TechnicianName { get; set; }

        [LIMSResourceDisplayName("Admin.Treatment.Temp")]
        public string Temp { get; set; }
        [LIMSResourceDisplayName("Admin.Treatment.Pulse")]
        public string Pulse { get; set; }

        [LIMSResourceDisplayName("Admin.Treatment.Respiration")]
        public string Respiration { get; set; }

        [LIMSResourceDisplayName("Admin.Treatment.ProvisionalDiagnosis")]
        public string ProvisionalDiagnosis { get; set; }


    }
}
