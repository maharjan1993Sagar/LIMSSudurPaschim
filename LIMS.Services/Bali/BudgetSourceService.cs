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
    public class BudgetSourceService:IBudgetSourceService
    {
        private readonly IRepository<BudgetSource> _BudgetSourceRepository;
        private readonly IMediator _mediator;
        public BudgetSourceService(IRepository<BudgetSource> BudgetSourceRepository, IMediator mediator)
        {
            _BudgetSourceRepository = BudgetSourceRepository;
            _mediator = mediator;
        }
        public async Task DeleteBudgetSource(BudgetSource BudgetSource)
        {
            if (BudgetSource == null)
                throw new ArgumentNullException("BudgetSource");

            await _BudgetSourceRepository.DeleteAsync(BudgetSource);

            //event notification
            await _mediator.EntityDeleted(BudgetSource);
        }
        public async Task<IPagedList<BudgetSource>> GetBudgetSource(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _BudgetSourceRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<BudgetSource>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<BudgetSource>> GetBudgetSource(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _BudgetSourceRepository.Table;
            //query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<BudgetSource>.Create(query, pageIndex, pageSize);
        }
        //public async Task<IPagedList<BudgetSource>> GetBudgetSource(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        //{
        //    var query = _BudgetSourceRepository.Table;
        //    query = query.Where(m => m.CreatedBy == createdby);
        //    if (!string.IsNullOrEmpty(district))
        //    {
        //        query = query.Where(
        //          m => m.District == district
        //        );
        //    }
        //    if (!string.IsNullOrEmpty(locallevel))
        //    {
        //        query = query.Where(
        //          m => m.LocalLevel == locallevel
        //        );
        //    }

        //    return await PagedList<BudgetSource>.Create(query, pageIndex, pageSize);
        //}

        public async Task<IPagedList<BudgetSource>> GetBudgetSource(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _BudgetSourceRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<BudgetSource>.Create(query, pageIndex, pageSize);
        }

        public Task<BudgetSource> GetBudgetSourceById(string id)
        {
            return _BudgetSourceRepository.GetByIdAsync(id);
        }

        public async Task InsertBudgetSource(BudgetSource BudgetSource)
        {
            if (BudgetSource == null)
                throw new ArgumentNullException("Livestock");

            await _BudgetSourceRepository.InsertAsync(BudgetSource);

            //event notification
            await _mediator.EntityInserted(BudgetSource);
        }

        public Task InsertBudgetSourceList(List<BudgetSource> BudgetSources)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateBudgetSource(BudgetSource BudgetSource)
        {
            if (BudgetSource == null)
                throw new ArgumentNullException("baliregister");

            await _BudgetSourceRepository.UpdateAsync(BudgetSource);

            //event notification
            await _mediator.EntityUpdated(BudgetSource);
        }

        public Task UpdateBudgetSourceList(List<BudgetSource> BudgetSources)
        {
            throw new NotImplementedException();
        }
    }
}
