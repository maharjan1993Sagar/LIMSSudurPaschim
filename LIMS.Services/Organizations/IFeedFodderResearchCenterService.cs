using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IFeedFodderResearchCenterService
    {
        Task<FeedFodderResearchCenter> GetFeedFodderResearchCenterById(string id);

        Task<IPagedList<FeedFodderResearchCenter>> GetFeedFodderResearchCenter(string createdby,string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FeedFodderResearchCenter>> GetFeedFodderResearchCenter(List<string> createdby, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteFeedFodderResearchCenter(FeedFodderResearchCenter feedFodderResearchCenter);

        Task InsertFeedFodderResearchCenterList(List<FeedFodderResearchCenter> feedFodderResearchCenter);
        Task InsertFeedFodderResearchCenter(FeedFodderResearchCenter feedFodderResearchCenter);
        Task UpdateFeedFodderResearchCenter(FeedFodderResearchCenter feedFodderResearchCenter);

    }
}
