using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public class BudgetService : IBudgetService
    {
        private readonly IRepository<Budget> _BudgetRepository;
        private readonly IMediator _mediator;
        public BudgetService(IRepository<Budget> BudgetRepository, IMediator mediator)
        {
            _BudgetRepository = BudgetRepository;
            _mediator = mediator;
        }
        public async Task DeleteBudget(Budget Budget)
        {
            if (Budget == null)
                throw new ArgumentNullException("Budget");

            await _BudgetRepository.DeleteAsync(Budget);

            //event notification
            await _mediator.EntityDeleted(Budget);
        }

        public async Task<IPagedList<Budget>> GetBudget(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;


            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<Budget>> GetBudget(string createdby,   string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;
            if (!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.PLIMBIS_No.Contains(keyword) || m.KarchaSrishak.Contains(keyword) || m.ActivityName.Contains(keyword));
            }
            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Budget>> GetBudgetSelect(string createdby, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //&& (m.kharchaCode == "22522" || m.kharchaCode == "26413" || m.kharchaCode == "26423" || m.kharchaCode == "31159"
            //|| m.kharchaCode == "22512"
            //));

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.PLIMBIS_No.Contains(keyword) || m.KarchaSrishak.Contains(keyword) || m.ActivityName.Contains(keyword));
            }
            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }



        public async Task<IPagedList<Budget>> GetBudget(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));

            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }

        //public async Task<IPagedList<Budget>> GetBudget(List<string> createdby, string programtype, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _BudgetRepository.Table;
        //    query = query.Where(m => createdby.Contains(m.CreatedBy)&&m.FiscalYear.Id==fiscalyear&&m.ProgramType==programtype&&m.Type==type);

        //    return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        //}

        public async Task<IPagedList<Budget>> GetBudget(
            string createdby,
            string fiscalYear,
            string sourceOfFund = "",
            string executionType = "",
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalYear);
            }
            if (!string.IsNullOrEmpty(sourceOfFund))
            {
                query = query.Where(m => m.SourceOfFund == sourceOfFund);
            }
            if (!string.IsNullOrEmpty(executionType))
            {
                query = query.Where(m => m.TypeOfExecution == executionType);
            }

            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }

        //public async Task<IPagedList<Budget>> GetNitigatKharakram(
        //  string createdby,
        // string fiscalYear,
        //  string programtype = "",
        //  string type = "",

        //  int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _BudgetRepository.Table;
        //    query = query.Where(m => m.CreatedBy == createdby && !string.IsNullOrEmpty(m.IsNitiTathaKaryaKram) && m.FiscalYear.Id == fiscalYear);
        //    if (!string.IsNullOrEmpty(programtype))
        //    {
        //        query = query.Where(m => m.ProgramType == programtype);
        //    }
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        query = query.Where(m => m.Type == type);
        //    }

        //    return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        //}
        public async Task<IPagedList<Budget>> GetMainKharakram(
          string createdby,
         string fiscalYear,
          string programtype = "",
          string type = "",

          int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //&&(m.PLIMBIS_No== "22512" || m.KarchaSrishak=="22522" || m.kharchaCode=="26413"||m.kharchaCode== "26423") && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.ExpensesCategory == programtype);
            }
            //if (!string.IsNullOrEmpty(type))
            //{
            //    query = query.Where(m => m.Type == type);
            //}

            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Budget>> GetBudget(
           List<string> createdby,
          string fiscalYear,
           string programtype = "",
           string type = "",

           int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _BudgetRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) && m.FiscalYearId == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.ExpensesCategory == programtype);
            }
            //if (!string.IsNullOrEmpty(type))
            //{
            //    query = query.Where(m => m.Type == type);
            //}

            return await PagedList<Budget>.Create(query, pageIndex, pageSize);
        }

        public Task<Budget> GetBudgetById(string Id)
        {
            return _BudgetRepository.GetByIdAsync(Id);

        }
        public async Task<bool> GetBudgetByLmBIsCode(string kharcha, string upakharcha, string fiscalYearId)
        {
            var query = _BudgetRepository.Table;
            query = query.Where(m => m.KarchaSrishak == kharcha  && m.KharchaUpaSirshak ==upakharcha && m.FiscalYearId == fiscalYearId);
            if (await query.CountAsync() > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<Budget> GetBudgetObjByLmBIsCode(string kharcha, string upakharcha, string fiscalYearId)
        {
            var query = _BudgetRepository.Table;
            query = query.Where(m => m.KarchaSrishak == kharcha && m.KharchaUpaSirshak == upakharcha && m.FiscalYearId == fiscalYearId);
            var budget = await query.FirstOrDefaultAsync();

            return budget ?? new Budget();

        }

        public async Task InsertBudget(Budget Budget)
        {
            if (Budget == null)
                throw new ArgumentNullException("Budget");

            await _BudgetRepository.InsertAsync(Budget);

            //event notification
            await _mediator.EntityInserted(Budget);
        }

        public async Task UpdateBudget(Budget Budget)
        {
            if (Budget == null)
                throw new ArgumentNullException("Budget");

            await _BudgetRepository.UpdateAsync(Budget);

            //event notification
            await _mediator.EntityUpdated(Budget);
        }

    }
}
