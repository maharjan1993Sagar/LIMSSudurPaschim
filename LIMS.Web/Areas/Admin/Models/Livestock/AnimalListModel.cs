using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Livestock
{
    public class AnimalListModel: BaseModel
    {
        [LIMSResourceDisplayName("Admin.Livestock.Animal.Keyword")]

        public string Keyword { get; set; }
        public string FarmId { get; set; }
    }


}
