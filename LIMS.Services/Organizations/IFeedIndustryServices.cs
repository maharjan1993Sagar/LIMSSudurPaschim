using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IFeedIndustryServices
    {
        Task<FeedIndustry> GetFeedIndustryById(string id);

        Task<IPagedList<FeedIndustry>> GetFeedIndustry(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteFeedIndustry(FeedIndustry feedIndustry);

        Task InsertFeedIndustryList(List<FeedIndustry> FeedIndustry);
        Task InsertFeedIndustry(FeedIndustry feedIndustry);
        Task UpdateFeedIndustry(FeedIndustry feedIndustry);
        Task<IPagedList<FeedIndustry>> GetFeedIndustryByType(string createdby, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FeedIndustry>> GetFeedIndustryByType(List<string> createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
