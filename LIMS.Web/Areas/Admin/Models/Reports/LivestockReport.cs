using LIMS.Core.ModelBinding;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public partial class LivestockReport
    {
       
        [LIMSResourceDisplayName("Admin.LIvestockReport.Month")]
        public string Month { get; set; }
        [LIMSResourceDisplayName("Admin.LIvestockReport.FiscalYear")]
        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.LIvestockReport.LocalLevel")]
        public string LocalLevel { get; set; }
        public string Species { get; set; }
        public List<string> AnimalTypes { get; set; }
        public List<BaseLivestockReportModel> Rows { get; set; }
       
        public class BaseLivestockReportModel
        {
            [LIMSResourceDisplayName("Admin.LIvestockReport.BreedType")]

            public string BreedType { get; set; }

            public List<int> AnimalTypeNo { get; set; }
        }

    


        //for wardwise report
        public class LivestockwardWiseReport
        {
            public LivestockwardWiseReport()
            {
                SpeciesList = new List<Species>();
            }
            public List<Species> SpeciesList { get; set; }
            public string Species { get; set; }
            public string BreedType { get; set; }
            public string Year { get; set; }
            public string Month { get; set; }
            public List<string> AnimalTypes { get; set; }
            public List<BaseLivestockWardWiseReport> Rows { get; set; }
        }
        public partial class BaseLivestockWardWiseReport
        {
            public string ward { get; set; }
            public List<int> AnimalTypeNo { get; set; }
        }



        public class LivestockReportByWard {

            public LivestockReportByWard()
            {
                this.Species = new List<SpeciesWithAnimalTypeModel>();
                this.AnimalTypes = new List<AnimalType>();
                this.Wards = new List<string>();
                this.BreedTypes = new List<string>();
                this.LivestockwardWiseReports = new List<LivestockwardWiseReport>();
            }
            public List<SpeciesWithAnimalTypeModel> Species { get; set; }
            public List<AnimalType> AnimalTypes { get; set; }
            public List<string> Wards { get; set; }

            public List<string> BreedTypes { get; set; }
            public List<LivestockwardWiseReport> LivestockwardWiseReports { get; set; }
            public string FiscalYear { get; set; }

        }


    }
}
