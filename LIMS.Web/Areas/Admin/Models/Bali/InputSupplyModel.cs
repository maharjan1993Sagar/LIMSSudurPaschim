using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class InputSupplyModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.InputSupply.Category")]
        public string CategoryName { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.Category")]
        public string CategoryId { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.Type")]

        public string Type { get; set; } //Krishi, Livestock
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Budget")]

        public string BudgetId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.PhoneNumber")]

        public string PhoneNumber { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Date")]

        public string NepaliDate { get; set; }

        public DateTime EnglishDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Month")]

        public string Month { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Purpose")]

        public string Purpose { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Quantity")]

        public string Quantity { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Unit")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.Price")]

        public Decimal Price { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.ProvidedBy")]

        public string ProvidedBy { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.ReceivedBy")]

        public string ReceivedBy { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.Bibaran")]

        public string Bibaran { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]

        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string VerifiedBy { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Remarks")]

        public string Remarks { get; set; }
        [LIMSResourceDisplayName("Admin.InputSupply.EntityId")]

        public string EntityId { get; set; }  // 
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
