using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class CalvingModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Calving.CalvingDate")]
        public DateTime CalvingDate { get; set; }

        [LIMSResourceDisplayName("Admin.Calving.CalvingType")]

        public string CalvingType { get; set; }
        [LIMSResourceDisplayName("Admin.Calving.StateOfCalving")]

        public string StateOfCalving { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Fiscalyear")]
        public string Fiscalyear { get; set; }
        [LIMSResourceDisplayName("Admin.Calving.TechnicianName")]
        public string TechnicianName { get; set; }
        [LIMSResourceDisplayName("Admin.Calving.BirthType")]
        public string BirthType { get; set; }
        [LIMSResourceDisplayName("Admin.Calving.EaseBirth")]
        public string EaseBirth { get; set; }
        [LIMSResourceDisplayName("Admin.Calving.FateOfCalf")]
        public string FateOfCalf { get; set; }





    }
}
