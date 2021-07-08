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
    public interface IImportantLinksService
    {
        Task<List<ImportantLinks>> GetAll();
        Task<IPagedList<ImportantLinks>> GetImportantLinks(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ImportantLinks>> GetImportantLinksByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteImportantLinks(ImportantLinks link);

        Task InsertImportantLinks(ImportantLinks link);

        Task UpdateImportantLinks(ImportantLinks link);
        Task<ImportantLinks> GetImportantLinksById(string Id);
    }
}
