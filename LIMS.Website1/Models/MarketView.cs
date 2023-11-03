using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class MarketView
    {
        public List<string> CropName { get; set; }

        public List<GetMarketData> Data { get; set; }

    }
    public class GetMarketData
    {
        public List<string> Data { get; set; }
    }

}
