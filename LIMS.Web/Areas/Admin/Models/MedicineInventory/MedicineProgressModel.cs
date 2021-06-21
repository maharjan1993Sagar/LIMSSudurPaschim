using LIMS.Core.ModelBinding;
using LIMS.Domain.Vaccination;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.MedicineInventory
{
    public class MedicineProgressModel
    {
        [LIMSResourceDisplayName("Admin.MedicineInventory.MedicineId")]
        public string MedicineId { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Quantity")]
        public string Quantity { get; set; }
        //  public string DeleiveredMedicine { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineInventory.Month")]
        public string Month { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineInventory.FiscalYearId")]
        public string FiscalYearId { get; set; }

        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineInventory.UnitId")]
        public string UnitId { get; set; }
        public List<VaccinationType> VaccinationType { get; set; }
        [UIHint("Date")]
        [LIMSResourceDisplayName("Admin.MedicineInventory.ReceivedDate")]

        public DateTime Date { get; set; }
        public string CreatdBy { get; set; }
    }
}
