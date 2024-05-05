using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IFertilizerShopService
    {
        Task<FertilizerShop> GetFertilizerShopById(string id);

        Task<IPagedList<FertilizerShop>> GetFertilizerShop(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FertilizerShop>> GetFertilizerShop(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteFertilizerShop(FertilizerShop FertilizerShop);
        Task InsertFertilizerShop(FertilizerShop FertilizerShop);
        Task InsertFertilizerShopList(List<FertilizerShop> FertilizerShop);
        Task UpdateFertilizerShop(FertilizerShop FertilizerShop);
        Task UpdateFertilizerShopList(List<FertilizerShop> FertilizerShop);
    }
}
