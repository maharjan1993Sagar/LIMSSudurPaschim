using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class FarmerModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Lims.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("Lims.Common.District")]


        public string District { get; set; }
        [LIMSResourceDisplayName("Lims.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Lims.Common.Address")]

        public string Address { get; set; }
        [LIMSResourceDisplayName("Lims.Common.WardNo")]

        public string WardNo { get; set; }
        [LIMSResourceDisplayName("Lims.Common.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.TalimName")]

        public string TalimId { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.IncuvationCenter")]

        public string IncuvationCenterId { get; set; }

        [LIMSResourceDisplayName("Lims.Common.Remarks")]

        public string Remarks { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.EnglishName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Male")]
        public string Male { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Female")]
        public string Female { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Dalit")]
        public string Dalit { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Janajati")]
        public string Janajati { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Yuba")]
        public string Yuba { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.Others")]
        public string Others { get; set; }

        [LIMSResourceDisplayName("Lims.Farmer.Sthar")]
        public string Sthar { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.Duration")]
        public string Duration { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.Purpose")]
        public string Purpose { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.PujigatKharchaKaryakramId")]

        public string pujigatKharchaKharakramId { get; set; }
        public string budgetId { get; set; }
        [LIMSResourceDisplayName("Lims.Talim.StartDate")]

        [UIHint("date")]
        public DateTime StartDate { get; set; }
        [UIHint("date")]
        [LIMSResourceDisplayName("Lims.Talim.EndDate")]

        public DateTime EndDate { get; set; }
        public string Gender { get; set; }
        public string Caste { get; set; }
        public string TotalExpense { get; set; }
        public string Logistics { get; set; }
    }
}
