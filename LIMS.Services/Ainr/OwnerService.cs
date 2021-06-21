using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public class OwnerService:IOwnerService
    {
        private readonly IRepository<Owner> _ownerRepository;
        private readonly IMediator _mediator;
        public OwnerService(IRepository<Owner> OwnerRepository, IMediator mediator)
        {
           _ownerRepository = OwnerRepository;
            _mediator = mediator;
        }

        public async Task DeleteOwner(Owner owner)
        {
            if (owner == null)
                throw new ArgumentNullException("Owner");
            await _ownerRepository.DeleteAsync(owner);

            //event notification
            await _mediator.EntityDeleted(owner);
        }

        public async Task<IPagedList<Owner>> GetOwner(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query =_ownerRepository.Table;
            return await PagedList<Owner>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Owner>> GetOwner(string farmid,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _ownerRepository.Table;
            query = query.Where(m => m.Farm.Id == farmid);
            return await PagedList<Owner>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Owner>> GetOwnerByFarmId(string farmId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query =_ownerRepository.Table;
            query = query.Where(d => d.FarmId == farmId);
            return await PagedList<Owner>.Create(query, pageIndex, pageSize);

        }

        public Task<Owner> GetOwnerById(string id)
        {
            return _ownerRepository.GetByIdAsync(id);
        }

        public async Task InsertOwner(Owner owner)
        {
            if (owner == null)
                throw new ArgumentNullException("Owner");
            await _ownerRepository.InsertAsync(owner);

            //event notification
            await _mediator.EntityInserted(owner);
        }

        public async Task UpdateOwner(Owner owner)
        {
            if (owner == null)
                throw new ArgumentNullException("Owner");
            await _ownerRepository.UpdateAsync(owner);

            //event notification
            await _mediator.EntityUpdated(owner);
        }

    }
}
