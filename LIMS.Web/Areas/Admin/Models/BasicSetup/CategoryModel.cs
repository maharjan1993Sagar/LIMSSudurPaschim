using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.BasicSetup
{
    public class CategoryModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Category.NameEnglish")]
        public string  NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Category.NameNepali")]
        public string NameNepali { get; set; }
        public string Type { get; set; }
    }
}
