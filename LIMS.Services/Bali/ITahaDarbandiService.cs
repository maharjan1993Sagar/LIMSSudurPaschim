using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ITahaDarbandiService
    {
        Task<DolfdTahaEntry> GetTahaDataById(string id);
        Task<IPagedList<DolfdTahaEntry>> GetTahaData(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<DolfdTahaEntry>> GetTahaData(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<DolfdTahaEntry>> GetTahaData(string createdby, string keyword, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<DolfdTahaEntry>> GetTahaData(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteTahaData(DolfdTahaEntry TahaData);

        Task InsertTahaData(DolfdTahaEntry TahaData);
        Task InsertTahaDataList(List<DolfdTahaEntry> livestocks);

        Task UpdateTahaData(DolfdTahaEntry TahaData);
        Task UpdateTahaDataList(List<DolfdTahaEntry> livestocks);

    }
}
