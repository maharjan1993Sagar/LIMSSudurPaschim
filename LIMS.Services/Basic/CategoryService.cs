using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
   public class CategoryService:ICategoryService
    {
        private readonly IRepository<Domain.BasicSetup.Category> _CategoryRepository;
        private readonly IMediator _mediator;
        public CategoryService(IRepository<Domain.BasicSetup.Category> CategoryRepository, IMediator mediator)
        {
            _CategoryRepository = CategoryRepository;
            _mediator = mediator;
        }
        public async Task DeleteCategory(Domain.BasicSetup.Category Category)
        {
            if (Category == null)
                throw new ArgumentNullException("Category");

            await _CategoryRepository.DeleteAsync(Category);

            //event notification
            await _mediator.EntityDeleted(Category);
        }

        public async Task<IPagedList<Domain.BasicSetup.Category>> GetCategory(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _CategoryRepository.Table;


            return await PagedList<Domain.BasicSetup.Category>.Create(query, pageIndex, pageSize);
        }

        public async Task<List<Domain.BasicSetup.Category>> GetCategoryByType(string type, string term)
        {
            var query = _CategoryRepository.Table;

            if (!String.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.Type == type);
            }
            if (!String.IsNullOrEmpty(term))
            {
                query = query.Where(m => m.NameEnglish.ToLower().Contains(term.Trim().ToLower())
                                            || m.NameNepali.ToLower().Contains(term.Trim().ToLower()));
            }
            return query.AsEnumerable().ToList();
        }

        public async Task<Domain.BasicSetup.Category> GetCategoryByName(string term)
        {
            var query = _CategoryRepository.Table;
           
           var category = query.FirstOrDefault(m => m.NameEnglish==term.Trim());
           
            return category;
        }

        public Task<Domain.BasicSetup.Category> GetCategoryById(string Id)
        {
            return _CategoryRepository.GetByIdAsync(Id);

        }

        public async Task InsertCategory(Domain.BasicSetup.Category Category)
        {
            if (Category == null)
                throw new ArgumentNullException("Category");

            await _CategoryRepository.InsertAsync(Category);

            //event notification
            await _mediator.EntityInserted(Category);
        }

        public async Task UpdateCategory(Domain.BasicSetup.Category Category)
        {
            if (Category == null)
                throw new ArgumentNullException("Category");

            await _CategoryRepository.UpdateAsync(Category);

            //event notification
            await _mediator.EntityUpdated(Category);
        }

    }
}
