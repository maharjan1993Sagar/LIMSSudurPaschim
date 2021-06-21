using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IDIaryIndustryService
    {
        Task<DiaryIndustryAndShop> GetDiaryIndustryAndShopById(string id);

        Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShop(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteDiaryIndustryAndShop(DiaryIndustryAndShop diaryIndustry);

        Task InsertDiaryIndustryAndShop(DiaryIndustryAndShop diaryIndustry);
        Task InsertDiaryIndustryAndShopList(List<DiaryIndustryAndShop> diaryIndustryAndShop);

        Task UpdateDiaryIndustryAndShop(DiaryIndustryAndShop diaryIndustry);
        Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShopByType(string createdby, string type, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShopByFiscalyear(string createdby, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShopByFiscalyear(List<string> createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
