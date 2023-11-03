using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IFarmerService
    {
        Task<Farmer> GetfarmerById(string id);
        Task<IPagedList<Farmer>> Getfarmer(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<Farmer>> Getfarmer(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        
        Task<IPagedList<Farmer>> GetfarmerByIncuvationCenter(string createdby,string keyword="",string fiscalyear="", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Farmer>> GetfarmerByIncuvationCenter(string createdby, string district,string talimname , string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Farmer>> GetfarmerByPugigatType(string createdby, string district, string talimname, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task Deletefarmer(Farmer farmer);

        Task Insertfarmer(Farmer farmer);
        Task InsertfarmerList(List<Farmer> livestocks);

        Task Updatefarmer(Farmer farmer);
        Task UpdatefarmerList(List<Farmer> livestocks);
    }
}
