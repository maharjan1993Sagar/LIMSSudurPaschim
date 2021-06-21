using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.RashanBalance
{
    public class FeedLibraryModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedClass")]
        public string FeedClass { get; set; }   // Concentrate or Roughage
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedType")]
        public string FeedType { get; set; }    // Dry or Green
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedTypeCategory")]
        public string FeedTypeCategory { get; set; }    // Legume or Non-legume (only if Green)
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedFor")]
        public string FeedFor { get; set; } // Ruminant or Non-ruminant
                                            
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.ConstituentMaxPercentageForCattle")]
        public decimal? ConstituentMaxPercentageForCattle { get; set; }// applicable only for Ruminant
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.ConstituentMaxPercentageForGoat")]

        public decimal? ConstituentMaxPercentageForGoat { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedNameEnglish")]

        public string FeedNameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedNameNepali")]

        public string FeedNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedNameScientific")]
        public string FeedNameScientific { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableDM")]

        public decimal AvailableDM { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableTDN")]
        public decimal AvailableTDN { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableCa")]

        public decimal AvailableCa { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableDCP")]
        public decimal AvailableDCP { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableP")]
        public decimal AvailableP { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableViatminA")]
        public decimal AvailableViatminA { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.FeedingSeason")]

        public string FeedingSeason { get; set; }
        [LIMSResourceDisplayName("Admin.RashanBalance.FeedLibrary.AvailableGeographicalArea")]

        public string AvailableGeographicalArea { get; set; }
    }
}
