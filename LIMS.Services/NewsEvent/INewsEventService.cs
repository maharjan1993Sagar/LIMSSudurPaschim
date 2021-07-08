using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.NewsEvent;
namespace LIMS.Services.NewsEvent
{
    public interface INewsEventService
    {
        Task<List<NewsEventTender>> GetAll();
        Task<NewsEventTender> GetNewsEventById(string Id);

        Task<IPagedList<NewsEventTender>> GetNewsEvent(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<NewsEventTender>> GetNewsEventByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteNewsEvent(NewsEventTender news);

        Task InsertNewsEvent(NewsEventTender news);

        Task UpdateNewsEvent(NewsEventTender news);

        Task UpdateNewsEvent(List<NewsEventTender> news);

     
    }
}
