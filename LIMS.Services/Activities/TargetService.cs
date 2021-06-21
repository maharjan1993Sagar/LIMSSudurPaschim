using LIMS.Domain;
using LIMS.Domain.Activities;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Activities
{
    public class TargetService:ITargetRegisterService
    {
        private readonly IRepository<TargetRegister> _targetRegisterRepository;
        private readonly IMediator _mediator;
        public TargetService(IRepository<TargetRegister> targetRegisterRepository, IMediator mediator)
        {
            _targetRegisterRepository = targetRegisterRepository;
            _mediator = mediator;
        }
        public async Task DeleteTargetRegister(TargetRegister targetRegister)
        {
            if (targetRegister == null)
                throw new ArgumentNullException("TargetRegister");

            await _targetRegisterRepository.DeleteAsync(targetRegister);

            //event notification
            await _mediator.EntityDeleted(targetRegister);
        }

        public async Task<IPagedList<TargetRegister>> GetTargetRegister(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _targetRegisterRepository.Table;


            return await PagedList<TargetRegister>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<TargetRegister>> GetFilteredTarget(string createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _targetRegisterRepository.Table;

            query = query.Where(m => m.CreatedBy == createdby && m.FiscalYearId == fiscalyear);

            return await PagedList<TargetRegister>.Create(query, pageIndex, pageSize);
        }
        public Task<TargetRegister> GetTargetRegisterById(string Id)
        {
            return _targetRegisterRepository.GetByIdAsync(Id);

        }

        public async Task InsertTargetRegister(TargetRegister targetRegister)
        {
            if (targetRegister == null)
                throw new ArgumentNullException("TargetRegister");

            await _targetRegisterRepository.InsertAsync(targetRegister);

            //event notification
            await _mediator.EntityInserted(targetRegister);
        }

        public async Task UpdateTargetRegister(TargetRegister targetRegister)
        {
            if (targetRegister == null)
                throw new ArgumentNullException("TargetRegister");

            await _targetRegisterRepository.UpdateAsync(targetRegister);

            //event notification
            await _mediator.EntityUpdated(targetRegister);
        }
        public async Task InsertTargetRegisterList(List<TargetRegister> targetRegisters)
        {
            if (targetRegisters.Count < 1)
                throw new ArgumentNullException("TargetRegister");
            await _targetRegisterRepository.InsertManyAsync(targetRegisters);


        }
        public async Task UpdateTargetRegisterList(List<TargetRegister> targetRegisters)
        {
            if (targetRegisters.Count < 1)
                throw new ArgumentNullException("TargetRegister");
            foreach (var item in targetRegisters)
            {
                await _targetRegisterRepository.UpdateAsync(item);
            }


        }

    }
}
