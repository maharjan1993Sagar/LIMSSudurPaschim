using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Livestock
{
    public class OwnerListModel:BaseModel
    {
        [LIMSResourceDisplayName("Admin.Livestock.Owner.Keyword")]
        public string Keyword { get; set; }
    }
}
