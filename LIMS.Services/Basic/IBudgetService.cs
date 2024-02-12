using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public interface IBudgetService
    {
        Task<Budget> GetBudgetById(string Id);
        Task<bool> GetBudgetByLmBIsCode(string kharcha, string upaKharcha, string fiscalYearId);
        Task<Budget> GetBudgetObjByLmBIsCode(string kharcha, string upaKharcha, string fiscalYearId);
        Task<IPagedList<Budget>> GetBudget(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Budget>> GetBudgetSelect(string createdby, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Budget>> GetBudget(string createdby,string keyword="", int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<Budget>> GetBudget(List<string> createdby,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Budget>> GetBudget(List<string> createdby, string fiscalyear, string sourceOfFund = "",string typeOfExpense = "", string ExpensesCategory = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Budget>> GetBudget(string createdby,string fiscalyear,string programtype="",string type="",string ExpensesCategory ="", string xetra = "", int pageIndex = 0, int pageSize = int.MaxValue);
        //Task<IPagedList<Budget>> GetNitigatKharakram(string createdby,
        //    string fiscalyear,
        //  string programtype = "",

        //  string type = "",

   //   int pageIndex = 0, int pageSize = int.MaxValue);
   //     Task<IPagedList<Budget>> GetMainKharakram(string createdby,
   //      string fiscalyear,
   //    string programtype = "",

   //    string type = "",

   //int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteBudget(Budget Budget);


        Task InsertBudget(Budget Budget);


        Task UpdateBudget(Budget Budget);
    }
}
