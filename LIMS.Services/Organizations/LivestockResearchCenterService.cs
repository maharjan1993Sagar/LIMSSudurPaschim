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
    public class LivestockResearchCenterService:ILivestockResearchCenterService
    {
        private readonly IRepository<LivestockResearchCenter> _livestockResearchCenterRepository;
        private readonly IMediator _mediator;
        public LivestockResearchCenterService(IRepository<LivestockResearchCenter> livestockResearchCenterRepository, IMediator mediator)
        {
            _livestockResearchCenterRepository = livestockResearchCenterRepository;
            _mediator = mediator;
        }
        public async Task DeleteLivestockResearchCenter(LivestockResearchCenter LivestockResearchCenter)
        {
            if (LivestockResearchCenter == null)
                throw new ArgumentNullException("LivestockResearchCenter");
            await _livestockResearchCenterRepository.DeleteAsync(LivestockResearchCenter);

            //event notification
            await _mediator.EntityDeleted(LivestockResearchCenter);
        }

        public async Task<IPagedList<LivestockResearchCenter>> GetLivestockResearchCenter(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockResearchCenterRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<LivestockResearchCenter>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<LivestockResearchCenter>> GetLivestockResearchCenter(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockResearchCenterRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<LivestockResearchCenter>.Create(query, pageIndex, pageSize);
        }
        public Task<LivestockResearchCenter> GetLivestockResearchCenterById(string id)
        {
            return _livestockResearchCenterRepository.GetByIdAsync(id);
        }

        public async Task InsertLivestockResearchCenter(LivestockResearchCenter LivestockResearchCenter)
        {
            if (LivestockResearchCenter == null)
                throw new ArgumentNullException("LivestockResearchCenter");
            await _livestockResearchCenterRepository.InsertAsync(LivestockResearchCenter);

            //event notification
            await _mediator.EntityInserted(LivestockResearchCenter);
        }
        public async Task InsertLivestockResearchCenterList(List<LivestockResearchCenter> LivestockResearchCenter)
        {
            if (LivestockResearchCenter == null)
                throw new ArgumentNullException("LivestockResearchCenter");
            await _livestockResearchCenterRepository.InsertManyAsync(LivestockResearchCenter);

        }

        public async Task UpdateLivestockResearchCenter(LivestockResearchCenter LivestockResearchCenter)
        {
            if (LivestockResearchCenter == null)
                throw new ArgumentNullException("LivestockResearchCenter");
            await _livestockResearchCenterRepository.UpdateAsync(LivestockResearchCenter);

            //event notification
            await _mediator.EntityUpdated(LivestockResearchCenter);
        }
    }
}
