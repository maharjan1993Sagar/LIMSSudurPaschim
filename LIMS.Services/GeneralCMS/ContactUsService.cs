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
  public   class ContactUsService:IContactUsService
    {
        private readonly IRepository<ContactUs> _contactUsRepository;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;

       public ContactUsService(IRepository<ContactUs> contactUsRepository, IMediator mediator,IWorkContext workContext)
        {
            _contactUsRepository = contactUsRepository;
            _mediator = mediator;
            _workContext = workContext;
        }
        public async Task<List<ContactUs>> GetAll()
        {
            var contactUs = _contactUsRepository.Table;
            return contactUs.ToList();
        }
        public async Task DeleteContact(ContactUs contact)
        {
            if (contact == null)
                throw new ArgumentNullException("ContactUs");
            await _contactUsRepository.DeleteAsync(contact);

            //event notification
            await _mediator.EntityDeleted(contact);
        }

        public async Task<IPagedList<ContactUs>> GetContactByUser(string userId,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _contactUsRepository.Collection;
            var filter = Builders<ContactUs>.Filter.Eq("UserId",userId);
            return await PagedList<ContactUs>.Create(query,filter, pageIndex, pageSize);
        }
        public async Task<IPagedList<ContactUs>> GetContact(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _contactUsRepository.Table;
            return await PagedList<ContactUs>.Create(query, pageIndex, pageSize);
        }
        
        public async Task<IPagedList<ContactUs>> GetContactByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _contactUsRepository.Collection;
            var filter = Builders<ContactUs>.Filter.Eq("UserId", userId);
            return await PagedList<ContactUs>.Create(query,filter, pageIndex, pageSize);
        }

        public Task<ContactUs> GetContactById(string Id)
        {
            return _contactUsRepository.GetByIdAsync(Id);
        }
        public async Task InsertContact(ContactUs contact)
        {
            if (contact == null)
                throw new ArgumentNullException("ContactUs");
            await _contactUsRepository.InsertAsync(contact);

            //event notification
            await _mediator.EntityInserted(contact);
        }

        public async Task UpdateContact(ContactUs contact)
        {
            if (contact == null)
                throw new ArgumentNullException("ContactUs");
            await _contactUsRepository.UpdateAsync(contact);

            //event notification
            await _mediator.EntityUpdated(contact);
        }

        public async Task UpdateContact(List<ContactUs> contact)
        {
            if (contact == null)
                throw new ArgumentNullException("ContactUs");

            await _contactUsRepository.UpdateAsync(contact);          
        }


    }


}
