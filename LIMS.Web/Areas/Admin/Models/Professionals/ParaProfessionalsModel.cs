using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.Professionals
{
  public  class ParaProfessionalsModel: BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Profession.ParaProfessional.EnglishName")]
        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Profession.ParaProfessional.NepaliName")]

        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Profession.ParaProfessional.Description")]

        public string Descriptions { get; set; }
    }
}
