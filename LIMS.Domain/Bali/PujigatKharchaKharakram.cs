using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class PujigatKharchaKharakram:BaseEntity
    {
        public string Limbis_Code { get; set; }
        public string Remarks { get; set; }
        public string Program { get; set; }
        public string ProgramSummery { get; set; }
        public string kharchaCode { get; set; }
        public string Unit { get; set; }
        public string YearlyParinam { get; set; }
        public string YearlyVar { get; set; }
        public string BarsikBajet { get; set; }
        public string BarshikBhar { get; set; }
        public string BarshikParinam { get; set; }
        public string PrathamChaumasikParimam { get; set; }
        public string PrathamChaumasikVar { get; set; }
        public string PrathamChaumasikBadjet { get; set; }
        public string DorsoChaumasikParimam { get; set; }
        public string DosroChaumasikVar { get; set; }
        public string DosroChaumasikBadjet { get; set; }
        public string TesroChaumasikParimam { get; set; }
        public string TesroChaumasikVar { get; set; }
        public string TesroChaumasikBadjet { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string Type { get; set; }
        public string ProgramType { get; set; }

    }
}