using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class SoilModel:BaseEntity
    {
        [LIMSResourceDisplayName("LIMS.Soil.SampleNo")]

        public string SampleNo { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.FarmerName")]

        public string FarmerName { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Tole")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Latitude")]

        public string Latitude { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Longitude")]

        public string Longitude { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.SoilStructure")]

        public string SoilStructure { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Ph")]

        public string Ph { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.PrangricPadartha")]

        public string PrangricPadartha { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Nitrogen")]

        public string Nitrogen { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Phosporus")]

        public string Phosporus { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Potas")]

        public string Potas { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.phoneNo")]

        public string phoneNo { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Zinc")]
        public string Zinc { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Boron")]
        public string Boron { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Othermicronutrients")]
        public string Othermicronutrients { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Unit")]
        public string Unit { get; set; }



    }
}
