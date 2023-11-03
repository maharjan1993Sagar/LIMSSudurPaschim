using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IDolfdAstaiPadService
    {
        Task<DolfdSthaiTahaEntry> GetTahaDataById(string id);
        Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(string createdby, string keyword, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteTahaData(DolfdSthaiTahaEntry TahaData);

        Task InsertTahaData(DolfdSthaiTahaEntry TahaData);
        Task InsertTahaDataList(List<DolfdSthaiTahaEntry> livestocks);

        Task UpdateTahaData(DolfdSthaiTahaEntry TahaData);
        Task UpdateTahaDataList(List<DolfdSthaiTahaEntry> livestocks);

    }
}
