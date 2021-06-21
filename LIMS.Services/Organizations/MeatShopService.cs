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
    public class MeatShopService:IMeatShopService
    {
        private readonly IRepository<MeatShop> _meatShopRepository;
        private readonly IMediator _mediator;
        public MeatShopService(IRepository<MeatShop> meatShopRepository, IMediator mediator)
        {
            _meatShopRepository = meatShopRepository;
            _mediator = mediator;
        }
        public async Task DeleteMeatShop(MeatShop meatShop)
        {
            if (meatShop == null)
                throw new ArgumentNullException("MeatShop");
            await _meatShopRepository.DeleteAsync(meatShop);

            //event notification
            await _mediator.EntityDeleted(meatShop);
        }

        public async Task<IPagedList<MeatShop>> GetMeatShop(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatShopRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<MeatShop>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<MeatShop>> GetMeatShop(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatShopRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<MeatShop>.Create(query, pageIndex, pageSize);
        }
        public async Task<List<MeatShop>> GetMeatShopByType(string createdby, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatShopRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.OtherOrganization.Type == type);
            return await PagedList<MeatShop>.Create(query, pageIndex, pageSize);
        }

        public Task<MeatShop> GetMeatShopById(string id)
        {
            return _meatShopRepository.GetByIdAsync(id);
        }

        public async Task InsertMeatShop(MeatShop MeatShop)
        {
            if (MeatShop == null)
                throw new ArgumentNullException("MeatShop");
            await _meatShopRepository.InsertAsync(MeatShop);

            //event notification
            await _mediator.EntityInserted(MeatShop);
        }
        public async Task InsertMeatShopList(List<MeatShop> MeatShop)
        {
            if (MeatShop == null)
                throw new ArgumentNullException("MeatShop");
            await _meatShopRepository.InsertManyAsync(MeatShop);

        }

        public async Task UpdateMeatShop(MeatShop MeatShop)
        {
            if (MeatShop == null)
                throw new ArgumentNullException("MeatShop");
            await _meatShopRepository.UpdateAsync(MeatShop);

            //event notification
            await _mediator.EntityUpdated(MeatShop);
        }


    }
}
