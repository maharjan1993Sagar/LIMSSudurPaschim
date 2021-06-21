using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
   public class UnitService:IUnitService
    {
        private readonly IRepository<Domain.BasicSetup.Unit> _unitRepository;
        private readonly IMediator _mediator;
        public UnitService(IRepository<Domain.BasicSetup.Unit> UnitRepository, IMediator mediator)
        {
            _unitRepository = UnitRepository;
            _mediator = mediator;
        }
        public async Task DeleteUnit(Domain.BasicSetup.Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("Unit");

            await _unitRepository.DeleteAsync(unit);

            //event notification
            await _mediator.EntityDeleted(unit);
        }

        public async Task<IPagedList<Domain.BasicSetup.Unit>> GetUnit(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _unitRepository.Table;


            return await PagedList<Domain.BasicSetup.Unit>.Create(query, pageIndex, pageSize);
        }

        public Task<Domain.BasicSetup.Unit> GetUnitById(string Id)
        {
            return _unitRepository.GetByIdAsync(Id);

        }

        public async Task InsertUnit(Domain.BasicSetup.Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("Unit");

            await _unitRepository.InsertAsync(unit);

            //event notification
            await _mediator.EntityInserted(unit);
        }

        public async Task UpdateUnit(Domain.BasicSetup.Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("Unit");

            await _unitRepository.UpdateAsync(unit);

            //event notification
            await _mediator.EntityUpdated(unit);
        }

    }
}
