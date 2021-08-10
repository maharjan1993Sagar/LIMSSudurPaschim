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
    public class MonthlyPragatiService : IMonthlyPragatiService
    {
        private readonly IRepository<MonthlyPragati> _MonthlyPragatiRepository;
        private readonly IMediator _mediator;
        public MonthlyPragatiService(IRepository<MonthlyPragati> MonthlyPragatiRepository, IMediator mediator)
        {
            _MonthlyPragatiRepository = MonthlyPragatiRepository;
            _mediator = mediator;
        }
        public async Task DeleteMonthlyPragati(MonthlyPragati MonthlyPragati)
        {
            if (MonthlyPragati == null)
                throw new ArgumentNullException("MonthlyPragati");

            await _MonthlyPragatiRepository.DeleteAsync(MonthlyPragati);

            //event notification
            await _mediator.EntityDeleted(MonthlyPragati);
        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(List<string> createdby, string fiscalYear, string programtype, string type, string month, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) && m.FiscalYear.Id == fiscalyear && m.Month == month && m.pujigatKharchaKharakram.ProgramType == programtype && m.pujigatKharchaKharakram.Type == type);
            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.FiscalYear.Id == fiscalYear && m.Month == month && m.pujigatKharchaKharakram.ProgramType == programtype && m.pujigatKharchaKharakram.Type == type);
            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, string type, string programType, string fiscalYear,string month, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby&&m.pujigatKharchaKharakram.Type==type&&m.pujigatKharchaKharakram.ProgramType==programType&&m.FiscalYear.Id==fiscalYear&&m.Month==month);


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<MonthlyPragati>> GetyearlyPragati(string createdby, string type, string programType, string fiscalYear,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.pujigatKharchaKharakram.Type == type && m.pujigatKharchaKharakram.ProgramType == programType && m.FiscalYear.Id == fiscalYear );


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }
        public Task<MonthlyPragati> GetMonthlyPragatiById(string id)
        {
            return _MonthlyPragatiRepository.GetByIdAsync(id);
        }

        public async Task InsertMonthlyPragati(MonthlyPragati MonthlyPragati)
        {
            if (MonthlyPragati == null)
                throw new ArgumentNullException("Livestock");

            await _MonthlyPragatiRepository.InsertAsync(MonthlyPragati);

            //event notification
            await _mediator.EntityInserted(MonthlyPragati);
        }

        public async Task InsertMonthlyPragatiList(List<MonthlyPragati> MonthlyPragatis)
        {
            if (MonthlyPragatis == null)
                throw new ArgumentNullException("Livestock");

            await _MonthlyPragatiRepository.InsertManyAsync(MonthlyPragatis);

        }

        public async Task UpdateMonthlyPragati(MonthlyPragati MonthlyPragati)
        {
            if (MonthlyPragati == null)
                throw new ArgumentNullException("baliregister");

            await _MonthlyPragatiRepository.UpdateAsync(MonthlyPragati);

            //event notification
            await _mediator.EntityUpdated(MonthlyPragati);
        }

        public async Task UpdateMonthlyPragatiList(List<MonthlyPragati> MonthlyPragatis)
        {
            if (MonthlyPragatis == null)
                throw new ArgumentNullException("MonthlyProgress");

            foreach (var item in MonthlyPragatis)
            {
                await _MonthlyPragatiRepository.UpdateAsync(item);
            }
        }
    }
}


