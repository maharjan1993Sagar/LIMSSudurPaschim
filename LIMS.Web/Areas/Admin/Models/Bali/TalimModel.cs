using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class TalimModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.Talim.EnglishName")]
        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.NepaliName")]

        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.StartDate")]
        [UIHint("date")]
        public DateTime? StartDate { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.EndDate")]
        [UIHint("date")]
        public DateTime? EndDate { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Lagat")]
        public string Lagat { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Duration")]

        public string Duration { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Fiscalyear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Description")]

        public string Description { get; set; }

    }
}
