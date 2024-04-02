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
    public class FertilizerShopService:IFertilizerShopService
    {
        private readonly IRepository<FertilizerShop> _FertilizerShopRepository;
        private readonly IMediator _mediator;
        public FertilizerShopService(IRepository<FertilizerShop> FertilizerShopRepository, IMediator mediator)
        {
            _FertilizerShopRepository = FertilizerShopRepository;
            _mediator = mediator;
        }
        public async Task DeleteFertilizerShop(FertilizerShop FertilizerShop)
        {
            if (FertilizerShop == null)
                throw new ArgumentNullException("FertilizerShop");
            await _FertilizerShopRepository.DeleteAsync(FertilizerShop);

            //event notification
            await _mediator.EntityDeleted(FertilizerShop);
        }

        public async Task<IPagedList<FertilizerShop>> GetFertilizerShop(string createdby,string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _FertilizerShopRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<FertilizerShop>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<FertilizerShop>> GetFertilizerShop(List<string> createdby, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _FertilizerShopRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<FertilizerShop>.Create(query, pageIndex, pageSize);
        }

        public Task<FertilizerShop> GetFertilizerShopById(string id)
        {
            return _FertilizerShopRepository.GetByIdAsync(id);
        }

        public async Task InsertFertilizerShop(FertilizerShop FertilizerShop)
        {
            if (FertilizerShop == null)
                throw new ArgumentNullException("FertilizerShop");
            await _FertilizerShopRepository.InsertAsync(FertilizerShop);

            //event notification
            await _mediator.EntityInserted(FertilizerShop);
        }
        public async Task InsertFertilizerShopList(List<FertilizerShop> FertilizerShop)
        {
            if (FertilizerShop == null)
                throw new ArgumentNullException("FertilizerShop");
            await _FertilizerShopRepository.InsertManyAsync(FertilizerShop);

        }

        public async Task UpdateFertilizerShop(FertilizerShop FertilizerShop)
        {
            if (FertilizerShop == null)
                throw new ArgumentNullException("FertilizerShop");
            await _FertilizerShopRepository.UpdateAsync(FertilizerShop);

            //event notification
            await _mediator.EntityUpdated(FertilizerShop);
        }
    }
}
