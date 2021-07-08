using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;

namespace LIMS.Services.GeneralCMS
{
    public interface IPageContentService
    {
        Task<List<PageContent>> GetAll();
        Task<IPagedList<PageContent>> GetPageContent(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PageContent>> GetPageContentByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeletePageContent(PageContent contact);

        Task InsertPageContent(PageContent contact);

        Task UpdatePageContent(PageContent contact);

        Task<PageContent> GetPageContentById(string id);

    }
}
