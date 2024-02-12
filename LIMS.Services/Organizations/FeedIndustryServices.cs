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
    public class FeedIndustryServices:IFeedIndustryServices
    {
        private readonly IRepository<FeedIndustry> _feedIndustryRepository;
        private readonly IMediator _mediator;
        public FeedIndustryServices(IRepository<FeedIndustry> feedIndustryRepository, IMediator mediator)
        {
            _feedIndustryRepository = feedIndustryRepository;
            _mediator = mediator;
        }
        public async Task DeleteFeedIndustry(FeedIndustry FeedIndustry)
        {
            if (FeedIndustry == null)
                throw new ArgumentNullException("FeedIndustry");
            await _feedIndustryRepository.DeleteAsync(FeedIndustry);

            //event notification
            await _mediator.EntityDeleted(FeedIndustry);
        }

        public async Task<IPagedList<FeedIndustry>> GetFeedIndustry(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedIndustryRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<FeedIndustry>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<FeedIndustry>> GetFeedIndustryByType(string createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedIndustryRepository.Table;
            if (!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            query = query.Where(m =>  m.OtherOrganization.Type == type);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<FeedIndustry>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FeedIndustry>> GetFeedIndustryByType(List<string> createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedIndustryRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy)  && m.OtherOrganization.Type == type);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<FeedIndustry>.Create(query, pageIndex, pageSize);
        }

        public Task<FeedIndustry> GetFeedIndustryById(string id)
        {
            return _feedIndustryRepository.GetByIdAsync(id);
        }

        public async Task InsertFeedIndustry(FeedIndustry FeedIndustry)
        {
            if (FeedIndustry == null)
                throw new ArgumentNullException("FeedIndustry");
            await _feedIndustryRepository.InsertAsync(FeedIndustry);

            //event notification
            await _mediator.EntityInserted(FeedIndustry);
        }
        public async Task InsertFeedIndustryList(List<FeedIndustry> FeedIndustry)
        {
            if (FeedIndustry == null)
                throw new ArgumentNullException("FeedIndustry");
            await _feedIndustryRepository.InsertManyAsync(FeedIndustry);

        }

        public async Task UpdateFeedIndustry(FeedIndustry FeedIndustry)
        {
            if (FeedIndustry == null)
                throw new ArgumentNullException("FeedIndustry");
            await _feedIndustryRepository.UpdateAsync(FeedIndustry);

            //event notification
            await _mediator.EntityUpdated(FeedIndustry);
        }

       
    }
}
