using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IMainActivityCodeService
    {
        Task<MainActivityCode> GetMainActivityCodeById(string id);
        //Task<IPagedList<MainActivityCode>> GetMainActivityCode(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MainActivityCode>> GetMainActivityCode(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<MainActivityCode>> GetMainActivityCode( int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<MainActivityCode>> GetMainActivityCode(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteMainActivityCode(MainActivityCode MainActivityCode);

        Task InsertMainActivityCode(MainActivityCode MainActivityCode);
        Task InsertMainActivityCodeList(List<MainActivityCode> livestocks);

        Task UpdateMainActivityCode(MainActivityCode MainActivityCode);
        Task UpdateMainActivityCodeList(List<MainActivityCode> livestocks);
        Task<Boolean> IsExistsCode(string code);
    }
}
