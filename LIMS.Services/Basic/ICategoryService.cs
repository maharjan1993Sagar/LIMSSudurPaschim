using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LIMS.Services.Basic
{
  public  interface ICategoryService
    {
        Task<Category> GetCategoryById(string Id);
        Task<IPagedList<Category>> GetCategory(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task<List<Category>> GetCategoryByType(string type,string term);
        Task<Category> GetCategoryByName(string term);
        Task<Category> GetCategoryByNameType(string term, string type);
        Task DeleteCategory(Category Category);


        Task InsertCategory(Category Category);


        Task UpdateCategory(Category Category);

    }
}
