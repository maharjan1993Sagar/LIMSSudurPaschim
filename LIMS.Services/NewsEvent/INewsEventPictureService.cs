using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.NewsEvent;
namespace LIMS.Services.NewsEvent
{
    public interface INewsEventPictureService
    {
        Task<NewsEventFile> GetPictureById(string Id);

        Task<IPagedList<NewsEventFile>> GetPicture(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeletePicture(NewsEventFile news);

        Task InsertPicture(NewsEventFile news);

        Task UpdatePicture(NewsEventFile news);


     
    }
}
