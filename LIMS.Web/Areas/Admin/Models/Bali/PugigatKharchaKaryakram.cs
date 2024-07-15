using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class PugigatKharchaKaryakramModel: BaseEntity
    {

        [LIMSResourceDisplayName("LIMS.Common.Limbis_Code")]
        public string Limbis_Code { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Remarks")]
        public string Remarks { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Program")]
        public string Program { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ProgramSummery")]
        public string ProgramSummery { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.kharchaCode")]
        public string kharchaCode { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Unit")]
        public string Unit { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.YearlyParinam")]
        public string YearlyParinam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.YearlyVar")]
        public string YearlyVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BarsikBajet")]
        public string BarsikBajet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BarshikBhar")]
        public string BarshikBhar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BarshikParinam")]
        public string BarshikParinam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PrathamChaumasikParimam")]
        public string PrathamChaumasikParimam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PrathamChaumasikVar")]
        public string PrathamChaumasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PrathamChaumasikBadjet")]
        public string PrathamChaumasikBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.DorsoChaumasikParimam")]
        public string DorsoChaumasikParimam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.DosroChaumasikVar")]
        public string DosroChaumasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.DosroChaumasikBadjet")]
        public string DosroChaumasikBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.TesroChaumasikParimam")]
        public string TesroChaumasikParimam { get; set; }
        public string TesroChaumasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.TesroChaumasikBadjet")]
        public string TesroChaumasikBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ChauthoTrimasikParimam")]

        public string ChauthoTrimasikParimam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ChauthoTrimasikVar")]

        public string ChauthoTrimasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ChauthoTrimasikkBadjet")]

        public string ChauthoTrimasikkBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Expenses_category")]
        public string Expenses_category { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ProgramType")]
        public string ProgramType { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.IsNitiTathaKaryaKram")]
        public string IsNitiTathaKaryaKram { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.IsTrainingKaryaKram")]
        public string IsTrainingKaryaKram { get; set; }

        [LIMSResourceDisplayName("LIMS.BudgetSource.Name")]
        public string BudgetSourceId { get; set; }
        [LIMSResourceDisplayName("LIMS.SubSector.Name")]
        public string SubSectorId { get; set; }

    }

    
}
