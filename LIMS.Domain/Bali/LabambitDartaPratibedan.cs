using LIMS.Domain.BesicSetup;
using LIMS.Domain.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class LabambitDartaPratibedan:BaseEntity
    {
        public PujigatKharchaKharakram PujigatKharchaKharakram { get; set; }
        public string PujigatKharchaKaryakramId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalyearId { get; set; }
        public string Male { get; set; }
        public string Female { get; set; }
        public string Dalit { get; set; }
        public string Janajati { get; set; }
        public string Aanya { get; set; }
        public Picture Picture { get; set; }
        public string PictureId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
