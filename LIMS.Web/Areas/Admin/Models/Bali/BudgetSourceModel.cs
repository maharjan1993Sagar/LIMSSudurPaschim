using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class BudgetSourceModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.BudgetSource.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Lims.BudgetSource.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Lims.BudgetSource.Description")]
        public string Description { get; set; }


    }
}
