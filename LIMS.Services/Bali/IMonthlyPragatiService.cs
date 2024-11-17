using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IMonthlyPragatiService
    {
        Task<MonthlyPragati> GetMonthlyPragatiById(string id);
        Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "",string budgetSourceId="", string subSectorId="");
        Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(List<string> createdby, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MonthlyPragati>> GetMonthlyPragati(string createdby, string type, string programType, string fiscalYear,string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<MonthlyPragati>> GetyearlyPragati(string createdby, string type, string programType, string fiscalYear, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<MonthlyPragati>> GetyearlyPragati(List<string> createdby, string type, string programType, string fiscalYear, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteMonthlyPragati(MonthlyPragati MonthlyPragati);


        Task InsertMonthlyPragati(MonthlyPragati MonthlyPragati);
        Task InsertMonthlyPragatiList(List<MonthlyPragati> livestocks);

        Task UpdateMonthlyPragati(MonthlyPragati MonthlyPragati);
        Task UpdateMonthlyPragatiList(List<MonthlyPragati> livestocks);

        Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(List<string> createdby,string fiscalYear,string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MonthlyPragati>> GetFilteredMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<MonthlyPragati>> GetFilteredNitiMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MonthlyPragati>> GetFilteredMainMonthlyPragati(string createdby, string fiscalYear, string programtype, string type, string month, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        
        Task<IPagedList<MonthlyPragati>> GetFilteredYearlyPragati(List<string> createdby, string fiscalYear, string programtype, string type, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MonthlyPragati>> GetFilteredYearlyPragati(string createdby, string fiscalYear, string programtype, string type, string budgetSourceId = "", string subSectorId = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");


    }
}
