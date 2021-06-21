using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs
{
    public class RationBalanceDto
    {
        public string FeedClass { get; set; }   // Concentrate or Roughage
        public string FeedType { get; set; }    // Dry or Green
        public string FeedTypeCategory { get; set; }    //legume  or Non-legume (only if Green)
        public string FeedFor { get; set; } // Ruminant or Non-ruminant   
        public decimal? ConstituentMaxPercentageForCattle { get; set; }  // applicable only for Ruminant
        public decimal? ConstituentMaxPercentageForGoat { get; set; }
        public string FeedNameEnglish { get; set; }
        public string FeedNameNepali { get; set; }
        public string FeedNameScientific { get; set; }
        public decimal AvailableDM { get; set; }
        public decimal AvailableTDN { get; set; }
        public decimal AvailableCa { get; set; }
        public decimal AvailableDCP { get; set; }
        public decimal AvailableP { get; set; }
        public decimal AvailableViatminA { get; set; }

    }
}
