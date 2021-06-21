using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Activities
{
    public class ActivityModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Activity.ActivityName")]
        public string ActivityName { get; set; }
        [LIMSResourceDisplayName("Admin.Activity.ActivityNameNepali")]
        public string ActivityNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Activity.Description")]
        public string Description { get; set; }
    }
}
