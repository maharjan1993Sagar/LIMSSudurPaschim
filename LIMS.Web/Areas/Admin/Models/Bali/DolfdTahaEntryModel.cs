using LIMS.Domain;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class DolfdTahaEntryModel:BaseEntity
    {
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
