using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IInputSupplyService
    {
        Task<InputSupply> GetInputSupplyById(string id);
        Task<IPagedList<InputSupply>> GetInputSupply(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<InputSupply>> GetInputSupply(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<InputSupply>> GetInputSupply(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteInputSupply(InputSupply InputSupply);

        Task InsertInputSupply(InputSupply InputSupply);
        Task InsertInputSupplyList(List<InputSupply> livestocks);

        Task UpdateInputSupply(InputSupply InputSupply);
        Task UpdateInputSupplyList(List<InputSupply> livestocks);
    }
}
