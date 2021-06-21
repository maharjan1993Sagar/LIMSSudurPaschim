using LIMS.Domain.Data;
using MediatR;
using System;
using System.Threading.Tasks;
using LIMS.Services.Events;
using LIMS.Domain;

namespace LIMS.Services.MoAMAC
{
    public partial class MoAMACService : IMoAMACService
    {
        private readonly IRepository<Domain.MoAMAC.MoAMAC> _moAMACRepository;
        private readonly IMediator _mediator;
        public MoAMACService(IRepository<Domain.MoAMAC.MoAMAC> moAMACRepository, IMediator mediator)
        {
            _moAMACRepository = moAMACRepository;
            _mediator = mediator;
        }

        public async Task DeleteMoAMAC(Domain.MoAMAC.MoAMAC moAMAC)
        {
            if (moAMAC == null)
                throw new ArgumentNullException("MoAMAC");
            await _moAMACRepository.DeleteAsync(moAMAC);

            //event notification
            await _mediator.EntityDeleted(moAMAC);
        }

        public async Task<IPagedList<Domain.MoAMAC.MoAMAC>> GetMoAMAC(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _moAMACRepository.Table;
            return await PagedList<Domain.MoAMAC.MoAMAC>.Create(query, pageIndex, pageSize);
        }

        public Task<Domain.MoAMAC.MoAMAC> GetMoAMACById(string id)
        {
            return _moAMACRepository.GetByIdAsync(id);
        }

        public async Task InsertMoAMAC(Domain.MoAMAC.MoAMAC moAMAC)
        {
            if (moAMAC == null)
                throw new ArgumentNullException("MoAMAC");
            await _moAMACRepository.InsertAsync(moAMAC);

            //event notification
            await _mediator.EntityInserted(moAMAC);
        }

        public async Task UpdateMoAMAC(Domain.MoAMAC.MoAMAC moAMAC)
        {
            if (moAMAC == null)
                throw new ArgumentNullException("MoAMAC");
            await _moAMACRepository.UpdateAsync(moAMAC);

            //event notification
            await _mediator.EntityUpdated(moAMAC);
        }
    }
}

