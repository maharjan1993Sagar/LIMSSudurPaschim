using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class PlantSoilManagementModel:BaseEntity
    {
        [LIMSResourceDisplayName("LIMS.Soil.PlantName")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.Temperature")]
        public string Temperature { get; set; }
        [LIMSResourceDisplayName("LIMS.Soil.AnualRainfall")]
        public string AnualRainfall { get; set; }
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
    }
}
