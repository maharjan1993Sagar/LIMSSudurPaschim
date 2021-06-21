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
    public class CanelClubeService:ICanelClubeService
    {
        private readonly IRepository<CanelClube> _canelClubeRepository;
        private readonly IMediator _mediator;
        public CanelClubeService(IRepository<CanelClube> canelClubeRepository, IMediator mediator)
        {
            _canelClubeRepository = canelClubeRepository;
            _mediator = mediator;
        }
        public async Task DeleteCanelClube(CanelClube CanelClube)
        {
            if (CanelClube == null)
                throw new ArgumentNullException("CanelClube");
            await _canelClubeRepository.DeleteAsync(CanelClube);

            //event notification
            await _mediator.EntityDeleted(CanelClube);
        }

        public async Task<IPagedList<CanelClube>> GetCanelClube(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _canelClubeRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            return await PagedList<CanelClube>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<CanelClube>> GetCanelClube(string createdby,string keyword, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _canelClubeRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if(!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.FiscalYear.Id == keyword);
            }
            return await PagedList<CanelClube>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<CanelClube>> GetCanelClubeByUserList(List<string> createdby, string keyword, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _canelClubeRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.FiscalYearId == keyword);
            }
            return await PagedList<CanelClube>.Create(query, pageIndex, pageSize);
        }
       public Task<CanelClube> GetCanelClubeById(string id)
        {
            return _canelClubeRepository.GetByIdAsync(id);
        }

        public async Task InsertCanelClube(CanelClube CanelClube)
        {
            if (CanelClube == null)
                throw new ArgumentNullException("CanelClube");
            await _canelClubeRepository.InsertAsync(CanelClube);

            //event notification
            await _mediator.EntityInserted(CanelClube);
        }
        public async Task InsertCanelClubeList(List<CanelClube> CanelClube)
        {
            if (CanelClube == null)
                throw new ArgumentNullException("CanelClube");
            await _canelClubeRepository.InsertManyAsync(CanelClube);

        }

        public async Task UpdateCanelClube(CanelClube CanelClube)
        {
            if (CanelClube == null)
                throw new ArgumentNullException("CanelClube");
            await _canelClubeRepository.UpdateAsync(CanelClube);

            //event notification
            await _mediator.EntityUpdated(CanelClube);
        }
    }
}
