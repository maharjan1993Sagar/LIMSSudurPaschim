using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class Farmer:BaseEntity
    {
        public string Name { get; set; }
        public string NameNepali { get; set; }

        public string Province { get; set; }

        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Talim Talim { get; set; }
        public string TalimId { get; set; }
        public IncubationCenter Incubation { get; set; }
        public string IncuvationCenterId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Remarks { get; set; }
        public string Male { get; set; }
        public string FeMale { get; set; }
        public string Dalit { get; set; }
        public string Janajati { get; set; }
        public string Others { get; set; }
        public string Ward { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public PujigatKharchaKharakram pujigatKharchaKharakram { get; set; }
        public string pujigatKharchaKharakramId { get; set; }
        public DateTime  StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Duration { get; set; }
        public string Purpose { get; set; }
    }
}
