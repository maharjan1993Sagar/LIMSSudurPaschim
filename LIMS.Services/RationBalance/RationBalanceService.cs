using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.RationBalance;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.RationBalance
{
    public class RationBalanceService : IRationBalanceService
    {
        private readonly IRepository<FeedLibrary> _feedLibraryRepository;
        private readonly IMediator _mediator;
        public RationBalanceService(IRepository<FeedLibrary> feedLibraryRepository, IMediator mediator)
        {
            _feedLibraryRepository = feedLibraryRepository;
            _mediator = mediator;
        }

        public async Task DeleteFeedLibrary(FeedLibrary feedLibrary)
        {
            if (feedLibrary == null)
                throw new ArgumentNullException("FeedLibrary");

            await _feedLibraryRepository.DeleteAsync(feedLibrary);

            //event notification
            await _mediator.EntityDeleted(feedLibrary);
        }

        public async Task<FeedLibrary> GetFeedLibraryById(string id)
        {
            return await _feedLibraryRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<FeedLibrary>> GetFeedLibraries(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query =  _feedLibraryRepository.Table;
            return await PagedList<FeedLibrary>.Create(query,pageIndex,pageSize);
        }

        public async Task InsertFeedLibrary(FeedLibrary feedLibrary)
        {
            if (feedLibrary == null)
                throw new ArgumentNullException("FeedLibrary");

            await _feedLibraryRepository.InsertAsync(feedLibrary);

            //event notification
            await _mediator.EntityInserted(feedLibrary);
        }

        public async Task InsertFeedLibraryList(IList<FeedLibrary> feedLibraries)
        {
            if (feedLibraries.Count < 1)
                throw new ArgumentNullException("FeedLibrary");
            await _feedLibraryRepository.InsertManyAsync(feedLibraries);
        }

        public async Task<IPagedList<FeedLibrary>> SearchFeedLibrary(string feedClass, string feedType, string feedTypeCategory, string feedFor, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedLibraryRepository.Table;
            if (!string.IsNullOrEmpty(feedClass))
                query = query.Where(fc => fc.FeedClass.Equals(feedClass));
            if (!string.IsNullOrEmpty(feedType))
                query = query.Where(fc => fc.FeedType.Equals(feedType));
            if (!string.IsNullOrEmpty(feedTypeCategory))
                query = query.Where(fc => fc.FeedTypeCategory.Equals(feedTypeCategory));
            if (!string.IsNullOrEmpty(feedFor))
                query = query.Where(fc => fc.FeedFor.Equals(feedFor));
            return await PagedList<FeedLibrary>.Create(query, pageIndex, pageSize);
        }

        public async Task UpdateFeedLibrary(FeedLibrary feedLibrary)
        {
            if (feedLibrary == null)
                throw new ArgumentNullException("FeedLibrary");

            await _feedLibraryRepository.UpdateAsync(feedLibrary);

            //event notification
            await _mediator.EntityUpdated(feedLibrary);
        }

        public async Task UpdateFeedLibraryList(IList<FeedLibrary> feedLibraries)
        {
            if (feedLibraries.Count < 1)
                throw new ArgumentNullException("FeedLibrary");
            foreach (var item in feedLibraries)
            {
                await _feedLibraryRepository.UpdateAsync(item);
            }
        }
    }
}
