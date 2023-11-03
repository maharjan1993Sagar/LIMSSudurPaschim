using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class DolfdTahaEntry:BaseEntity
    {
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Remarks { get; set; }

        public string twelvethpadD { get; set; }
        public string twelvethpadPurti { get; set; }

        public string eleventhpad { get; set; }
        public string eleventhpadPurti { get; set; }

        public string tenthpad { get; set; }
        public string tenthpadPurti { get; set; }

        public string eightthpad { get; set; }
        public string eightthpadPurti { get; set; }

        public string sixthpad { get; set; }
        public string sixthpadPurti { get; set; }

        public string fourthpad { get; set; }
        public string fourththpadPurti { get; set; }

        public string TahaBihin { get; set; }
        public string TahaBihinpadPurti { get; set; }
        public string TahaBihinNas { get; set; }
        public string TahaBihinNaspadPurti { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }

    }
}
