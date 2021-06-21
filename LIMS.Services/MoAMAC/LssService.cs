using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.MoAMAC;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public class LssService : ILssService
    {
        private readonly IRepository<Lss> _lssRepository;
        private readonly IMediator _mediator;
        public LssService(IRepository<Lss> lssRepository, IMediator mediator)
        {
            _lssRepository = lssRepository;
            _mediator = mediator;
        }
        public async Task DeleteLss(Lss lss)
        {
            if (lss == null)
                throw new ArgumentNullException("Lss");
            await _lssRepository.DeleteAsync(lss);

            //event notification
            await _mediator.EntityDeleted(lss);
        }

        public async Task<IPagedList<Lss>> GetLss(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _lssRepository.Table;
            return await PagedList<Lss>.Create(query, pageIndex, pageSize);
        }

        public Task<Lss> GetLssById(string id)
        {
            return _lssRepository.GetByIdAsync(id);
        }

        public async Task InsertLss(Lss lss)
        {
            if (lss == null)
                throw new ArgumentNullException("Lss");
            await _lssRepository.InsertAsync(lss);

            //event notification
            await _mediator.EntityInserted(lss);
        }

        public async Task UpdateLss(Lss lss)
        {
            if (lss == null)
                throw new ArgumentNullException("Lss");
            await _lssRepository.UpdateAsync(lss);

            //event notification
            await _mediator.EntityUpdated(lss);
        }
        public virtual async Task<IPagedList<Lss>> GetLssByVhlsecId(string vhlsecId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _lssRepository.Table;
            query = query.Where(l => l.VhlsecId == vhlsecId);
            return await PagedList<Lss>.Create(query, pageIndex, pageSize);
        }

        public async Task UpdateLss(List<Lss> lss)
        {
            if (lss == null)
                throw new ArgumentNullException("Lss");
            await _lssRepository.UpdateAsync(lss);
        }
    }
}
