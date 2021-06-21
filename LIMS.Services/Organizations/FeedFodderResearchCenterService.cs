using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public class FeedFodderResearchCenterService:IFeedFodderResearchCenterService
    {
        private readonly IRepository<FeedFodderResearchCenter> _feedFodderResearchCenterRepository;
        private readonly IMediator _mediator;
        public FeedFodderResearchCenterService(IRepository<FeedFodderResearchCenter> feedFodderResearchCenterRepository, IMediator mediator)
        {
            _feedFodderResearchCenterRepository = feedFodderResearchCenterRepository;
            _mediator = mediator;
        }
        public async Task DeleteFeedFodderResearchCenter(FeedFodderResearchCenter FeedFodderResearchCenter)
        {
            if (FeedFodderResearchCenter == null)
                throw new ArgumentNullException("FeedFodderResearchCenter");
            await _feedFodderResearchCenterRepository.DeleteAsync(FeedFodderResearchCenter);

            //event notification
            await _mediator.EntityDeleted(FeedFodderResearchCenter);
        }

        public async Task<IPagedList<FeedFodderResearchCenter>> GetFeedFodderResearchCenter(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedFodderResearchCenterRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<FeedFodderResearchCenter>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FeedFodderResearchCenter>> GetFeedFodderResearchCenter(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedFodderResearchCenterRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<FeedFodderResearchCenter>.Create(query, pageIndex, pageSize);
        }
        public Task<FeedFodderResearchCenter> GetFeedFodderResearchCenterById(string id)
        {
            return _feedFodderResearchCenterRepository.GetByIdAsync(id);
        }

        public async Task InsertFeedFodderResearchCenter(FeedFodderResearchCenter FeedFodderResearchCenter)
        {
            if (FeedFodderResearchCenter == null)
                throw new ArgumentNullException("FeedFodderResearchCenter");
            await _feedFodderResearchCenterRepository.InsertAsync(FeedFodderResearchCenter);

            //event notification
            await _mediator.EntityInserted(FeedFodderResearchCenter);
        }
        public async Task InsertFeedFodderResearchCenterList(List<FeedFodderResearchCenter> FeedFodderResearchCenter)
        {
            if (FeedFodderResearchCenter == null)
                throw new ArgumentNullException("FeedFodderResearchCenter");
            await _feedFodderResearchCenterRepository.InsertManyAsync(FeedFodderResearchCenter);

        }

        public async Task UpdateFeedFodderResearchCenter(FeedFodderResearchCenter FeedFodderResearchCenter)
        {
            if (FeedFodderResearchCenter == null)
                throw new ArgumentNullException("FeedFodderResearchCenter");
            await _feedFodderResearchCenterRepository.UpdateAsync(FeedFodderResearchCenter);

            //event notification
            await _mediator.EntityUpdated(FeedFodderResearchCenter);
        }
    }
}
