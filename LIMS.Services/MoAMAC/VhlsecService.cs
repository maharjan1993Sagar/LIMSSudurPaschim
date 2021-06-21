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
    public class VhlsecService : IVhlsecService
    {
        private readonly IRepository<Vhlsec> _vhlsecRepository;
        private readonly IMediator _mediator;
        public VhlsecService(IRepository<Vhlsec> vhlsecRepository, IMediator mediator)
        {
            _vhlsecRepository = vhlsecRepository;
            _mediator = mediator;
        }
        public async Task DeleteVhlsec(Vhlsec vhlsec)
        {
            if (vhlsec == null)
                throw new ArgumentNullException("Vhlsec");
            await _vhlsecRepository.DeleteAsync(vhlsec);

            //event notification
            await _mediator.EntityDeleted(vhlsec);
        }
        public virtual async Task<IPagedList<Vhlsec>> GetVhlsecByDolfdId(string dolfdId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vhlsecRepository.Table;
            query = query.Where(l => l.Dolfd.Id == dolfdId);
            return await PagedList<Vhlsec>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<Vhlsec>> GetVhlsec(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vhlsecRepository.Table;
            return await PagedList<Vhlsec>.Create(query, pageIndex, pageSize);
        }

        public Task<Vhlsec> GetVhlsecById(string id)
        {
            return _vhlsecRepository.GetByIdAsync(id);
        }

        public async Task InsertVhlsec(Vhlsec vhlsec)
        {
            if (vhlsec == null)
                throw new ArgumentNullException("Vhlsec");
            await _vhlsecRepository.InsertAsync(vhlsec);

            //event notification
            await _mediator.EntityInserted(vhlsec);
        }

        public async Task UpdateVhlsec(Vhlsec vhlsec)
        {
            if (vhlsec == null)
                throw new ArgumentNullException("Vhlsec");
            await _vhlsecRepository.UpdateAsync(vhlsec);

            //event notification
            await _mediator.EntityUpdated(vhlsec);
        }

        

        public async Task UpdateVhlsec(List<Vhlsec> vhlsec)
        {
            if (vhlsec == null)
                throw new ArgumentNullException("Vhlsec");
            await _vhlsecRepository.UpdateAsync(vhlsec);
        }
    }
}
