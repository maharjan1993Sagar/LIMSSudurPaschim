using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models
{
    public class MedicineReportModel
    {
       
        [LIMSResourceDisplayName("Admin.MedicineReport.FiscalYear")]
        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineReport.Month")]
        public string Month { get; set; }
        public IList<BaseMedicineReportModel> BaseMedicineReportModels { get; set; }
    }
    public class BaseMedicineReportModel {
        [LIMSResourceDisplayName("Admin.MedicineReport.MedicineName")]
        public string MedicineName { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineReport.Unit")]
        public string Unit { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineReport.TotalStock")]
        public string TotalStock { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineReport.Distribution")]
        public string Distribution { get; set; }
        [LIMSResourceDisplayName("Admin.MedicineReport.RemainingStock")]
        public string RemaningStock { get; set; }

        public string Remarks { get; set; }


    }
}
