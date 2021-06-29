using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public class PujigatKharchaKharakramService:IPujigatKharchaKharakramService
    {
        private readonly IRepository<PujigatKharchaKharakram> _pujigatKharchaKharakramRepository;
        private readonly IMediator _mediator;
        public PujigatKharchaKharakramService(IRepository<PujigatKharchaKharakram> pujigatKharchaKharakramRepository, IMediator mediator)
        {
            _pujigatKharchaKharakramRepository = pujigatKharchaKharakramRepository;
            _mediator = mediator;
        }
        public async Task DeletePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            if (pujigatKharchaKharakram == null)
                throw new ArgumentNullException("pujigatKharchaKharakram");

            await _pujigatKharchaKharakramRepository.DeleteAsync(pujigatKharchaKharakram);

            //event notification
            await _mediator.EntityDeleted(pujigatKharchaKharakram);
        }

        public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;


            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }

        public Task<PujigatKharchaKharakram> GetPujigatKharchaKharakramById(string Id)
        {
            return _pujigatKharchaKharakramRepository.GetByIdAsync(Id);

        }

        public async Task InsertPujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            if (pujigatKharchaKharakram == null)
                throw new ArgumentNullException("pujigatKharchaKharakram");

            await _pujigatKharchaKharakramRepository.InsertAsync(pujigatKharchaKharakram);

            //event notification
            await _mediator.EntityInserted(pujigatKharchaKharakram);
        }

        public async Task UpdatePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            if (pujigatKharchaKharakram == null)
                throw new ArgumentNullException("pujigatKharchaKharakram");

            await _pujigatKharchaKharakramRepository.UpdateAsync(pujigatKharchaKharakram);

            //event notification
            await _mediator.EntityUpdated(pujigatKharchaKharakram);
        }

    }
}
