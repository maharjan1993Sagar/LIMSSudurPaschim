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
    public interface IGalleryService
    {
        Task<List<Gallery>> GetAll();
        Task<IPagedList<Gallery>> GetGallery(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Gallery>> GetGalleryByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteGallery(Gallery gallery);

        Task InsertGallery(Gallery gallery);

        Task UpdateGallery(Gallery gallery);
        Task<Gallery> GetGalleryById(string id);
        Task DeletePicture(NewsEventFile farmPicture);

        Task InsertPicture(NewsEventFile farmPicture);

        Task UpdatePicture(NewsEventFile farmPicture);

    }
}
