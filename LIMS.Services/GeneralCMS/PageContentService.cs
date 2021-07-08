using LIMS.Core;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.GeneralCMS
{
  public   class PageContentService:IPageContentService
    {
        private readonly IRepository<PageContent> _pageContentRepository;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;

       public PageContentService(IRepository<PageContent> pageContentRepository, IMediator mediator,IWorkContext workContext)
        {
            _pageContentRepository = pageContentRepository;
            _mediator = mediator;
            _workContext = workContext;
        }

        public async Task<List<PageContent>> GetAll()
        {
            var pageContent = _pageContentRepository.Table;
            return pageContent.ToList();
        }
        public async Task DeletePageContent(PageContent pageContent)
        {
            if (pageContent == null)
                throw new ArgumentNullException("PageContent");
            await _pageContentRepository.DeleteAsync(pageContent);

            //event notification
            await _mediator.EntityDeleted(pageContent);
        }

        public async Task<IPagedList<PageContent>> GetPageContent(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pageContentRepository.Table;
            return await PagedList<PageContent>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<PageContent>> GetPageContentByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _pageContentRepository.Collection;
            var filter = Builders<PageContent>.Filter.Eq("UserId", userId);
            return await PagedList<PageContent>.Create(query,filter, pageIndex, pageSize);
        }

        public Task<PageContent> GetPageContentById(string Id)
        {
            return _pageContentRepository.GetByIdAsync(Id);
        }
        public async Task InsertPageContent(PageContent pageContent)
        {
            if (pageContent == null)
                throw new ArgumentNullException("PageContent");
            await _pageContentRepository.InsertAsync(pageContent);

            //event notification
            await _mediator.EntityInserted(pageContent);
        }

        public async Task UpdatePageContent(PageContent pageContent)
        {
            if (pageContent == null)
                throw new ArgumentNullException("PageContent");
            await _pageContentRepository.UpdateAsync(pageContent);

            //event notification
            await _mediator.EntityUpdated(pageContent);
        }

        public async Task UpdatePageContent(List<PageContent> pageContent)
        {
            if (pageContent == null)
                throw new ArgumentNullException("PageContent");

            await _pageContentRepository.UpdateAsync(pageContent);          
        }

       
    }


}
