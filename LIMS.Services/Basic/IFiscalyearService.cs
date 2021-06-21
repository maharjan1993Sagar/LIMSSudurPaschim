using LIMS.Domain;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
   public interface IFiscalYearService
    {
        Task<FiscalYear> GetFiscalYearById(string Id);
        Task<IPagedList<FiscalYear>> GetFiscalYear(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteFiscalYear(FiscalYear fiscalYear);


        Task InsertFiscalYear(FiscalYear fiscalYear);


        Task UpdateFiscalYear(FiscalYear fiscalYear);
        Task UpdateFiscalYear(List<FiscalYear> fiscalYear);
         Task<FiscalYear> GetCurrentFiscalYear();



    }
}
