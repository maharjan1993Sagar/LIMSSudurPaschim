using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class DolfdShthaiTahaEntryModel
    {

        [LIMSResourceDisplayName("Lims.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        public string Remarks { get; set; }

        public string twelvethpad { get; set; }
        public string eleventhpad { get; set; }
        public string tenthpad { get; set; }
        public string eightthpad { get; set; }
        public string sixthpad { get; set; }
        public string fourthpad { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
