using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.BasicSetup
{
    public class DiseaseModel:BaseEntity
    {
        public Guid DiseaseId { get; set; }
        [LIMSResourceDisplayName("Admin.Disease.NepaliName")]
        public string DiseaseNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Disease.EnglishName")]

        public string DiseaseNameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Disease.Symptoms")]
        public string Symptoms { get; set; }
        [LIMSResourceDisplayName("Admin.Disease.ShortName")]
        public string ShortName { get; set; }

    }
}
