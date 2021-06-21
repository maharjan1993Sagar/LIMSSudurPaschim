using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Nutration
{
    public class RationBalancingModel
    {
          [LIMSResourceDisplayName("Admin.Ration.Species")]
        public string Species { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.Breed")]
        public string Breed { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.MilkingStatus")]
        public string MilkingStatus { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.Pregnent")]
        public string Pregnent { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.Pregnency")]
        public string Pregnency { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.PregnencyMonth")]
        public string PregnencyMonth { get; set; }

        [LIMSResourceDisplayName("Admin.Ration.WeightKg")]
        public string WeightKg { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.MilkVolume")]
        public string MilkVolume { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.MilkFat")]
        public string MilkFat { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.Snf")]
        public string Snf { get; set; }
        [LIMSResourceDisplayName("Admin.Ration.Snf")]
        public string MilkPrice { get; set; }
    }
}
