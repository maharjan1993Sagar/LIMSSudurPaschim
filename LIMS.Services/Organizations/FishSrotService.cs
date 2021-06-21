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
    public class FishSrotService:IFishSrotService
    {
        private readonly IRepository<FishSrot> _fishSrotRepository;
        private readonly IMediator _mediator;
        public FishSrotService(IRepository<FishSrot> fishSrotRepository, IMediator mediator)
        {
            _fishSrotRepository = fishSrotRepository;
            _mediator = mediator;
        }
        public async Task DeleteFishSrot(FishSrot FishSrot)
        {
            if (FishSrot == null)
                throw new ArgumentNullException("FishSrot");
            await _fishSrotRepository.DeleteAsync(FishSrot);

            //event notification
            await _mediator.EntityDeleted(FishSrot);
        }

        public async Task<IPagedList<FishSrot>> GetFishSrot(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fishSrotRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<FishSrot>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<FishSrot>> GetFishSrot(List<string> createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fishSrotRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<FishSrot>.Create(query, pageIndex, pageSize);
        }


        public Task<FishSrot> GetFishSrotById(string id)
        {
            return _fishSrotRepository.GetByIdAsync(id);
        }

        public async Task InsertFishSrot(FishSrot FishSrot)
        {
            if (FishSrot == null)
                throw new ArgumentNullException("FishSrot");
            await _fishSrotRepository.InsertAsync(FishSrot);

            //event notification
            await _mediator.EntityInserted(FishSrot);
        }
        public async Task InsertFishSrotList(List<FishSrot> FishSrot)
        {
            if (FishSrot == null)
                throw new ArgumentNullException("FishSrot");
            await _fishSrotRepository.InsertManyAsync(FishSrot);

        }

        public async Task UpdateFishSrot(FishSrot FishSrot)
        {
            if (FishSrot == null)
                throw new ArgumentNullException("FishSrot");
            await _fishSrotRepository.UpdateAsync(FishSrot);

            //event notification
            await _mediator.EntityUpdated(FishSrot);
        }

    }
}
