using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;
using LIMS.Domain.NewsEvent;

namespace LIMS.Services.GeneralCMS
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAll();
        Task<IPagedList<Banner>> GetBanner(int pageIndex = 0, int pageSize = int.MaxValue);
       
        Task DeleteBanner(Banner gallery);

        Task InsertBanner(Banner gallery);

        Task UpdateBanner(Banner gallery);
        Task<Banner> GetBannerById(string id);
        Task<IPagedList<Banner>> GetBannerByUser(int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
