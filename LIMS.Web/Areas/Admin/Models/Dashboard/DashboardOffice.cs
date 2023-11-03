using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Core.ModelBinding;
using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
namespace LIMS.Web.Areas.Admin.Models.Dashboard
{
    public class DashboardOffice
    {
        public decimal FinencialPercent { get; set; }
        public decimal Budget { get; set; }
        public decimal Progress { get; set; }
        public string NoOfMaleBenifiries { get; set; }
        public string NoOfFeMaleBenifiries { get; set; }
        public string TotalSubsidiesAmount { get; set; }
        [LIMSResourceDisplayName("Lims.Common.FiscalYear")]
        public string Fiscalyear { get; set; }
        public string Fy { get; set; }

        public string MaleTraining { get; set; }
        public string FemaleTraining { get; set; }
        public List<AanudanKokaryakram> Aanudans { get; set; }
        public List<MolmacData> MolmacDatas { get; set; }

    }
    public class MolmacData
    {
        public string Name { get; set; }
        public string YearlyBudget { get; set; }
        public string ExpancesTillDate { get; set; }
        public string FinencialProgress { get; set; }
    }
}
