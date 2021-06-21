using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public interface IFiscalYearForGraphService
    {
        Task<FiscalYearForGraphSetup> GetFiscalYear();


        Task InsertFiscalYear(FiscalYearForGraphSetup fiscalYear);


        Task UpdateFiscalYear(FiscalYearForGraphSetup fiscalYear);
        Task<FiscalYearForGraphSetup> GetFiscalYearById(string Id);
    }
}
