using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IBudgetSourceService
    {
        Task<BudgetSource> GetBudgetSourceById(string id);
       // Task<IPagedList<BudgetSource>> GetBudgetSource(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<BudgetSource>> GetBudgetSource(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<BudgetSource>> GetBudgetSource( int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<BudgetSource>> GetBudgetSource(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteBudgetSource(BudgetSource BudgetSource);

        Task InsertBudgetSource(BudgetSource BudgetSource);
        Task InsertBudgetSourceList(List<BudgetSource> livestocks);

        Task UpdateBudgetSource(BudgetSource BudgetSource);
        Task UpdateBudgetSourceList(List<BudgetSource> livestocks);
    }
}
