using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IMarketDataService
    {
        Task<MarketData> GetmarketDataById(string id);
        Task<IPagedList<MarketData>> GetmarketData(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MarketData>> GetmarketData(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MarketData>> GetmarketData(string createdby, string keyword ,int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<MarketData>> GetmarketData(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeletemarketData(MarketData marketData);

        Task InsertmarketData(MarketData marketData);
        Task InsertmarketDataList(List<MarketData> livestocks);

        Task UpdatemarketData(MarketData marketData);
        Task UpdatemarketDataList(List<MarketData> livestocks);

    }
}
