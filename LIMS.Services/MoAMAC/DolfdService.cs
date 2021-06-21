using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.MoAMAC;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public class DolfdService : IDolfdService
    {
        private readonly IRepository<Dolfd> _dolfdRepository;
        private readonly IMediator _mediator;
        public DolfdService(IRepository<Dolfd> dolfdRepository, IMediator mediator)
        {
            _dolfdRepository = dolfdRepository;
            _mediator = mediator;
        }

        public async Task DeleteDolfd(Dolfd dolfd)
        {
            if (dolfd == null)
                throw new ArgumentNullException("Dolfd");
            await _dolfdRepository.DeleteAsync(dolfd);

            //event notification
            await _mediator.EntityDeleted(dolfd);
        }

        public async Task<IPagedList<Dolfd>> GetDolfd(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _dolfdRepository.Table;
            return await PagedList<Dolfd>.Create(query, pageIndex, pageSize);
        }

        public Task<Dolfd> GetDolfdById(string id)
        {
            return _dolfdRepository.GetByIdAsync(id);
        }

        public async Task InsertDolfd(Dolfd dolfd)
        {
            if (dolfd == null)
                throw new ArgumentNullException("Dolfd");
            await _dolfdRepository.InsertAsync(dolfd);

            //event notification
            await _mediator.EntityInserted(dolfd);
        }

        public async Task UpdateDolfd(Dolfd dolfd)
        {
            if (dolfd == null)
                throw new ArgumentNullException("Dolfd");
            await _dolfdRepository.UpdateAsync(dolfd);

            //event notification
            await _mediator.EntityUpdated(dolfd);
        }

        public async Task<IPagedList<Dolfd>> GetDolfdByMolmacId(string molmacId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _dolfdRepository.Table;
            query = query.Where(d => d.MoamacId == molmacId);
            return await PagedList<Dolfd>.Create(query, pageIndex, pageSize);
        }

        public async Task UpdateDolfd(List<Dolfd> dolfd)
        {
            if (dolfd == null)
                throw new ArgumentNullException("Dolfd");
            await _dolfdRepository.UpdateAsync(dolfd);
        }
    }
}
