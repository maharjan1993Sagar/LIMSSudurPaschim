using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IMeatShopService
    {
        Task<MeatShop> GetMeatShopById(string id);

        Task<IPagedList<MeatShop>> GetMeatShop(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<MeatShop>> GetMeatShop(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteMeatShop(MeatShop meatShop);

        Task InsertMeatShopList(List<MeatShop> MeatShop);
        Task InsertMeatShop(MeatShop meatShop);
        Task UpdateMeatShop(MeatShop meatShop);
        Task<List<MeatShop>> GetMeatShopByType(string createdby, string type, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
