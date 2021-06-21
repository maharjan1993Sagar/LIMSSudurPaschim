using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.MoAMAC;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public class NlboService:INlboService
    {
        private readonly IRepository<Nlbo> _nlboRepository;
        private readonly IMediator _mediator;
        public NlboService(IRepository<Nlbo> nlboRepository, IMediator mediator)
        {
            _nlboRepository = nlboRepository;
            _mediator = mediator;
        }

        public async Task DeleteNlbo(Nlbo nlbo)
        {
            if (nlbo == null)
                throw new ArgumentNullException("Nlbo");
            await _nlboRepository.DeleteAsync(nlbo);

            //event notification
            await _mediator.EntityDeleted(nlbo);
        }

        public async Task<IPagedList<Nlbo>> GetNlbo(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _nlboRepository.Table;
            return await PagedList<Nlbo>.Create(query, pageIndex, pageSize);
        }

        public Task<Nlbo> GetNlboById(string id)
        {
            return _nlboRepository.GetByIdAsync(id);
        }

        public async Task InsertNlbo(Nlbo nlbo)
        {
            if (nlbo == null)
                throw new ArgumentNullException("Nlbo");
            await _nlboRepository.InsertAsync(nlbo);

            //event notification
            await _mediator.EntityInserted(nlbo);
        }

        public async Task UpdateNlbo(Nlbo nlbo)
        {
            if (nlbo == null)
                throw new ArgumentNullException("Nlbo");
            await _nlboRepository.UpdateAsync(nlbo);

            //event notification
            await _mediator.EntityUpdated(nlbo);
        }

     

        

    }
}
