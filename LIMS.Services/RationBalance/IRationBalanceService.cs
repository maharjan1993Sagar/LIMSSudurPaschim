using LIMS.Domain;
using LIMS.Domain.RationBalance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.RationBalance
{
    public partial interface IRationBalanceService
    {
        Task<FeedLibrary> GetFeedLibraryById(string id);
        Task<IPagedList<FeedLibrary>> GetFeedLibraries(int pageIndex = 0,int PageSize=int.MaxValue);
        Task<IPagedList<FeedLibrary>> SearchFeedLibrary(string feedClass, string feedType, string feedTypeCategory, string feedFor, int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteFeedLibrary(FeedLibrary feedLibrary);
        Task InsertFeedLibrary(FeedLibrary feedLibrary);
        Task InsertFeedLibraryList(IList<FeedLibrary> feedLibraries);
        Task UpdateFeedLibrary(FeedLibrary feedLibrary);
        Task UpdateFeedLibraryList(IList<FeedLibrary> feedLibraries);
    }
}
