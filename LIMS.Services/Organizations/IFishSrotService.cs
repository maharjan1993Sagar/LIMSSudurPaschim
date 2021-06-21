using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IFishSrotService
    {

        Task<FishSrot> GetFishSrotById(string id);

        Task<IPagedList<FishSrot>> GetFishSrot(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FishSrot>> GetFishSrot(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteFishSrot(FishSrot fishSrot);

        Task InsertFishSrotList(List<FishSrot> FishSrot);
        Task InsertFishSrot(FishSrot fishSrot);
        Task UpdateFishSrot(FishSrot fishSrot);

    }
}
