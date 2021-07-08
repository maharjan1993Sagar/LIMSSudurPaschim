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
    public class BaliRegisterService : IBaliRegisterService
    {
        private readonly IRepository<BaliRegister> _baliRegisterRepository;
        private readonly IMediator _mediator;
        public BaliRegisterService(IRepository<BaliRegister> baliRegisterRepository, IMediator mediator)
        {
            _baliRegisterRepository = baliRegisterRepository;
            _mediator = mediator;
        }
        public async Task DeletebaliRegister(BaliRegister baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("BaliRegister");

            await _baliRegisterRepository.DeleteAsync(baliRegister);

            //event notification
            await _mediator.EntityDeleted(baliRegister);
        }

        public async Task<IPagedList<BaliRegister>> GetbaliRegister(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _baliRegisterRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<BaliRegister>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<BaliRegister>> GetbaliRegister(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _baliRegisterRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<BaliRegister>.Create(query, pageIndex, pageSize);
        }

        public Task<BaliRegister> GetbaliRegisterById(string id)
        {
            return _baliRegisterRepository.GetByIdAsync(id);
        }

        public async Task InsertbaliRegister(BaliRegister baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("Livestock");

            await _baliRegisterRepository.InsertAsync(baliRegister);

            //event notification
            await _mediator.EntityInserted(baliRegister);
        }

        public Task InsertbaliRegisterList(List<BaliRegister> baliRegisters)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatebaliRegister(BaliRegister baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("baliregister");

            await _baliRegisterRepository.UpdateAsync(baliRegister);

            //event notification
            await _mediator.EntityUpdated(baliRegister);
        }

        public Task UpdatebaliRegisterList(List<BaliRegister> baliRegisters)
        {
            throw new NotImplementedException();
        }
    }
}
