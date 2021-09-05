using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class MarketModel:BaseEntity
    {
        [LIMSResourceDisplayName("LIMS.Market.BreedName")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.SpeciesName")]

        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.Month")]

        public string Month { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.MinPrice")]
        [UIHint("nullabledecimal")]
        public string MinPrice { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.MaxPrice")]
        [UIHint("nullabledecimal")]
        public string MaxPrice { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.WholesalePrice")]
        [UIHint("nullabledecimal")]
        public string WholesalePrice { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.FarmGetPrice")]
        [UIHint("nullabledecimal")]
        public string FarmGetPrice { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.NameOfMarket")]


        public string NameOfMarket { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.Address")]


        public string AddressBazar { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.FiscalYear")]

        public string FiscalYearId { get; set; }

        [LIMSResourceDisplayName("LIMS.Market.UnitId")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("LIMS.Market.RecordingDate")]
        [UIHint("date")]
        public DateTime RecordingDate { get; set; }


    }
}
