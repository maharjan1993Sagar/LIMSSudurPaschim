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
  public class ImportantLinksService:IImportantLinksService
    {
        private readonly IRepository<ImportantLinks> _linkRepository;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;

       public ImportantLinksService(IRepository<ImportantLinks> linkRepository, IMediator mediator,IWorkContext workContext)
        {
            _linkRepository = linkRepository;
            _mediator = mediator;
            _workContext = workContext;
        }

        public async Task<List<ImportantLinks>> GetAll()
        {
            var links = _linkRepository.Table;
            return links.ToList();
        }
        public async Task DeleteImportantLinks(ImportantLinks link)
        {
            if (link == null)
                throw new ArgumentNullException("ImportantLinks");
            await _linkRepository.DeleteAsync(link);

            //event notification
            await _mediator.EntityDeleted(link);
        }

        public async Task<IPagedList<ImportantLinks>> GetImportantLinks(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _linkRepository.Table;
            return await PagedList<ImportantLinks>.Create(query, pageIndex, pageSize);
        } 
        public async Task<IPagedList<ImportantLinks>> GetImportantLinksByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _linkRepository.Collection;
            var filter = Builders<ImportantLinks>.Filter.Eq("UserId", userId);
            return await PagedList<ImportantLinks>.Create(query,filter, pageIndex, pageSize);
        }

        public Task<ImportantLinks> GetImportantLinksById(string Id)
        {
            return _linkRepository.GetByIdAsync(Id);
        }
        public async Task InsertImportantLinks(ImportantLinks link)
        {
            if (link == null)
                throw new ArgumentNullException("ImportantLinks");
            await _linkRepository.InsertAsync(link);

            //event notification
            await _mediator.EntityInserted(link);
        }

        public async Task UpdateImportantLinks(ImportantLinks link)
        {
            if (link == null)
                throw new ArgumentNullException("ImportantLinks");
            await _linkRepository.UpdateAsync(link);

            //event notification
            await _mediator.EntityUpdated(link);
        }

        public async Task UpdateImportantLinks(List<ImportantLinks> link)
        {
            if (link == null)
                throw new ArgumentNullException("ImportantLinks");

            await _linkRepository.UpdateAsync(link);          
        }

       
    }


}
