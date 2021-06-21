using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.Professionals
{
  public  class VetGraduateModel: BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Profession.VetGraduate.EnglishName")]

        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Profession.VetGraduate.NepaliName")]

        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Profession.VetGraduate.Description")]

        public string Descriptions { get; set; }
    }
}
