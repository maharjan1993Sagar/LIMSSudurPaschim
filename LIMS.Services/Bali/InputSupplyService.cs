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
    public class InputSupplyService:IInputSupplyService
    {
        private readonly IRepository<InputSupply> _InputSupplyRepository;
        private readonly IMediator _mediator;
        public InputSupplyService(IRepository<InputSupply> InputSupplyRepository, IMediator mediator)
        {
            _InputSupplyRepository = InputSupplyRepository;
            _mediator = mediator;
        }
        public async Task DeleteInputSupply(InputSupply InputSupply)
        {
            if (InputSupply == null)
                throw new ArgumentNullException("InputSupply");

            await _InputSupplyRepository.DeleteAsync(InputSupply);

            //event notification
            await _mediator.EntityDeleted(InputSupply);
        }
        public async Task<IPagedList<InputSupply>> GetInputSupply(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _InputSupplyRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<InputSupply>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<InputSupply>> GetInputSupply(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _InputSupplyRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(
                  m => m.CreatedBy == createdby
                );
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(
                  m => m.FiscalYearId == fiscalYear
                );
            }
            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(
                  m => m.District == district
                );
            }
            if (!string.IsNullOrEmpty(locallevel))
            {
                query = query.Where(
                  m => m.LocalLevel == locallevel
                );
            }

            return await PagedList<InputSupply>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<InputSupply>> GetInputSupply(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _InputSupplyRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<InputSupply>.Create(query, pageIndex, pageSize);
        }

        public Task<InputSupply> GetInputSupplyById(string id)
        {
            return _InputSupplyRepository.GetByIdAsync(id);
        }

        public async Task InsertInputSupply(InputSupply InputSupply)
        {
            if (InputSupply == null)
                throw new ArgumentNullException("Livestock");

            await _InputSupplyRepository.InsertAsync(InputSupply);

            //event notification
            await _mediator.EntityInserted(InputSupply);
        }

        public Task InsertInputSupplyList(List<InputSupply> InputSupplys)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInputSupply(InputSupply InputSupply)
        {
            if (InputSupply == null)
                throw new ArgumentNullException("baliregister");

            await _InputSupplyRepository.UpdateAsync(InputSupply);

            //event notification
            await _mediator.EntityUpdated(InputSupply);
        }

        public Task UpdateInputSupplyList(List<InputSupply> InputSupplys)
        {
            throw new NotImplementedException();
        }
    }
}
