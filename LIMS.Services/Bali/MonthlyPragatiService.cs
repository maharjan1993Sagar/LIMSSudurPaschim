﻿using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public class MonthlyPragatiService : IMonthlyPragatiService
    {
        private readonly IRepository<MonthlyPragati> _MonthlyPragatiRepository;
        private readonly IRepository<MainActivityCode> _mainActivityCodeRepository;

        private readonly IMediator _mediator;
        public MonthlyPragatiService(IRepository<MonthlyPragati> MonthlyPragatiRepository, IRepository<MainActivityCode> mainActivityCodeRepository,
            IMediator mediator)
        {
            _MonthlyPragatiRepository = MonthlyPragatiRepository;
            _mainActivityCodeRepository = mainActivityCodeRepository;
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

        //public async Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(List<string> createdby, string fiscalYear, string programtype, string type, string month, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        //{
        //    var query = _MonthlyPragatiRepository.Table;
        //    query = query.Where(m => createdby.Contains(m.CreatedBy) && m.FiscalYear.Id == fiscalyear && m.Month == month && m.pujigatKharchaKharakram.ProgramType == programtype && m.pujigatKharchaKharakram.Type == type);
        //    return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        //}
        public async Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;

            query = query.Where(m => m.CreatedBy == createdby && m.FiscalYear.Id == fiscalYear);
            if (!String.IsNullOrEmpty(month))
            {
                query = query.Where(m => m.Month == month); 
            }

            if (!string.IsNullOrEmpty(programtype))
            {
                if (programtype.ToLower() == "subsidy")
                {
                    query = query.Where(m => m.pujigatKharchaKharakram.Expenses_category == programtype.ToLower());

                }
                if (programtype.ToLower() == "training")
                {
                query = query.Where(m => !String.IsNullOrEmpty(m.pujigatKharchaKharakram.IsTrainingKaryaKram) && m.pujigatKharchaKharakram.IsTrainingKaryaKram == programtype.ToLower());
                }
                if (programtype.ToLower() == "niti")
                 { 
                query = query.Where(m => !String.IsNullOrEmpty(m.pujigatKharchaKharakram.IsNitiTathaKaryaKram) && m.pujigatKharchaKharakram.IsNitiTathaKaryaKram =="yes");
                }
            }
           
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }
      
        public async Task<IPagedList<MonthlyPragati>> GetFilteredNitiMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && !string.IsNullOrEmpty(m.pujigatKharchaKharakram.IsNitiTathaKaryaKram) && m.FiscalYear.Id == fiscalYear && m.Month == month);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredMainMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            var queryCodes = _mainActivityCodeRepository.Table;
            var mainActivity = queryCodes.Select(m => m.Limbis_Code)
                                           .Distinct().ToList();

            query = query.Where(m => m.CreatedBy == createdby&& mainActivity.Contains(m.pujigatKharchaKharakram.kharchaCode) && m.FiscalYearId == fiscalYear && m.Month == month);
           // query = query.Where(m => m.CreatedBy == createdby&& (m.pujigatKharchaKharakram.kharchaCode == "22512" || m.pujigatKharchaKharakram.kharchaCode == "22522" || m.pujigatKharchaKharakram.kharchaCode == "26413" || m.pujigatKharchaKharakram.kharchaCode == "26423") && m.FiscalYear.Id == fiscalYear && m.Month == month);
            if (!string.IsNullOrEmpty(programtype))
            {
                if (!string.IsNullOrEmpty(programtype))
                {
                    if (programtype.ToLower() == "subsidy")
                    {
                        query = query.Where(m => m.pujigatKharchaKharakram.Expenses_category.ToLower() == programtype.ToLower());

                    }
                    if (programtype.ToLower() == "training")
                    {
                        query = query.Where(m => !String.IsNullOrEmpty(m.pujigatKharchaKharakram.IsTrainingKaryaKram) && m.pujigatKharchaKharakram.IsTrainingKaryaKram.ToLower() == programtype.ToLower());
                    }
                    if (programtype.ToLower() == "niti")
                    {
                        query = query.Where(m => !String.IsNullOrEmpty(m.pujigatKharchaKharakram.IsNitiTathaKaryaKram) && m.pujigatKharchaKharakram.IsNitiTathaKaryaKram.ToLower() == "yes");
                    }
                }
            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }
            if (!string.IsNullOrEmpty(month))
            {
                query = query.Where(m => m.Month == month);
            }

            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(List<string> createdby, string fiscalYear, string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            var queryCodes = _mainActivityCodeRepository.Table;
            var mainActivity = queryCodes.Select(m => m.Limbis_Code)
                                           .Distinct().ToList();

            query = query.Where(m => createdby.Contains(m.CreatedBy)&& mainActivity.Contains(m.pujigatKharchaKharakram.kharchaCode) && m.FiscalYear.Id == fiscalYear && m.Month == month);
           // query = query.Where(m => createdby.Contains(m.CreatedBy)&& string.IsNullOrEmpty(m.pujigatKharchaKharakram.IsNitiTathaKaryaKram)&& !(m.pujigatKharchaKharakram.kharchaCode == "22512" || m.pujigatKharchaKharakram.kharchaCode == "22522" || m.pujigatKharchaKharakram.kharchaCode == "26413" || m.pujigatKharchaKharakram.kharchaCode == "26423") && m.FiscalYear.Id == fiscalYear && m.Month == month);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }



        public async Task<IPagedList<MonthlyPragati>> GetFilteredYearlyPragati(string createdby, string fiscalYear, string programtype, string type,  int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "", string budgetSourceId = "", string subSectorId = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<MonthlyPragati>> GetFilteredYearlyPragati(List<string> createdby, string fiscalYear, string programtype, string type,  int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "", string budgetSourceId = "", string subSectorId = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }









        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);

            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "", string budgetSourceId = "", string subSectorId = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, string type, string programType, string fiscalYear, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.FiscalYear.Id == fiscalYear );
            if (!string.IsNullOrEmpty(month))
            {
                query = query.Where(m =>  m.Month == month);

            }
            if (!string.IsNullOrEmpty(programType))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programType);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }

            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<MonthlyPragati>> GetyearlyPragati(string createdby, string type, string programType, string fiscalYear, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programType))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programType);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }

            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);

        }
       
        public async Task<IPagedList<MonthlyPragati>> GetyearlyPragati(List<string> createdby, string type, string programType, string fiscalYear, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programType))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programType);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }

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

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "", string budgetSourceId="", string subSectorId="")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(List<string> createdby, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy ) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }

            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredNitiMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && !string.IsNullOrEmpty(m.pujigatKharchaKharakram.IsNitiTathaKaryaKram) && m.FiscalYear.Id == fiscalYear && m.Month == month);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            //if (!string.IsNullOrEmpty(budgetSourceId))
            //{
            //    query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            //}
            //if (!string.IsNullOrEmpty(subSectorId))
            //{
            //    query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            //}
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredYearlyPragati(List<string> createdby, string fiscalYear, string programtype, string type, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MonthlyPragati>> GetFilteredYearlyPragati(string createdby, string fiscalYear, string programtype, string type, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MonthlyPragatiRepository.Table;
            query = query.Where(m => m.CreatedBy== createdby && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.ProgramType == programtype);

            }
            if (!string.IsNullOrEmpty(budgetSourceId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.BudgetSourceId == budgetSourceId);
            }
            if (!string.IsNullOrEmpty(subSectorId))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.SubSectorId == subSectorId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.pujigatKharchaKharakram.Type == type);
            }


            return await PagedList<MonthlyPragati>.Create(query, pageIndex, pageSize);
        }

       
    }
}


