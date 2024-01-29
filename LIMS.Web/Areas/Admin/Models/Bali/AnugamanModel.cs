using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class AnugamanModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Common.Category")]

        public string CategoryName { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Category")]

        public string CategoryId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.ObservedBy")]

        public string ObservedBy { get; set; } //which item is selected
        [LIMSResourceDisplayName("Admin.Anugaman.FarmerId")]

        public string FarmerId { get; set; } //Entity Id
        [LIMSResourceDisplayName("Admin.Anugaman.Name")]

        public string Name { get; set; } //Entity Id
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Type")]

        public string Type { get; set; } //Krishi, Livestock
        [LIMSResourceDisplayName("Admin.Common.Date")]

        public string NepaliDate { get; set; }
        public DateTime EnglishDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Month")]

        public string Month { get; set; }
        [LIMSResourceDisplayName("Admin.Anugaman.Observations")]

        public string Observations { get; set; }
        [LIMSResourceDisplayName("Admin.Anugaman.Suggestion")]

        public string Suggestion { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Remarks")]

        public string Remarks { get; set; }
        public IList<string> ImageUrls { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]

        public string Ward { get; set; }

        public string District { get; set; }
        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.Common.PhoneNumber")]

        public string PhoneNumber { get; set; }
        [LIMSResourceDisplayName("Admin.Common.VerifiedBy")]

        public string VerifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
