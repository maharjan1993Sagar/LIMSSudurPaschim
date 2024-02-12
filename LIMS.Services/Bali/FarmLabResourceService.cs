using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public class FarmLabResourceService:IFarmLabResourceService
    {
        private readonly IRepository<FarmLabResources> _farmLabResourcesRepository;
        private readonly IMediator _mediator;
        public FarmLabResourceService(IRepository<FarmLabResources> farmLabResourcesRepository, IMediator mediator)
        {
            _farmLabResourcesRepository = farmLabResourcesRepository;
            _mediator = mediator;
        }
        public async Task DeletefarmLabResources(FarmLabResources farmLabResources)
        {
            if (farmLabResources == null)
                throw new ArgumentNullException("FarmLabResources");

            await _farmLabResourcesRepository.DeleteAsync(farmLabResources);

            //event notification
            await _mediator.EntityDeleted(farmLabResources);
        }

        public async Task<IPagedList<FarmLabResources>> GetfarmLabResources(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _farmLabResourcesRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<FarmLabResources>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FarmLabResources>> GetfarmLabResources(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _farmLabResourcesRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<FarmLabResources>.Create(query, pageIndex, pageSize);
        }

        public Task<FarmLabResources> GetfarmLabResourcesById(string id)
        {
            return _farmLabResourcesRepository.GetByIdAsync(id);
        }

        public async Task InsertfarmLabResources(FarmLabResources farmLabResources)
        {
            if (farmLabResources == null)
                throw new ArgumentNullException("Livestock");

            await _farmLabResourcesRepository.InsertAsync(farmLabResources);

            //event notification
            await _mediator.EntityInserted(farmLabResources);
        }

        public Task InsertfarmLabResourcesList(List<FarmLabResources> farmLabResourcess)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatefarmLabResources(FarmLabResources farmLabResources)
        {
            if (farmLabResources == null)
                throw new ArgumentNullException("baliregister");

            await _farmLabResourcesRepository.UpdateAsync(farmLabResources);

            //event notification
            await _mediator.EntityUpdated(farmLabResources);
        }

        public Task UpdatefarmLabResourcesList(List<FarmLabResources> farmLabResourcess)
        {
            throw new NotImplementedException();
        }
    }
}
