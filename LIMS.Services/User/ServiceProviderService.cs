using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.User;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.User
{
    public class ServiceProviderService:IServiceProviderService
    {
        private readonly IRepository<ServiceProvider> _serviceProviderRepository;
        private readonly IMediator _mediator;
        public ServiceProviderService(IRepository<ServiceProvider> serviceProviderRepository, IMediator mediator)
        {
            _serviceProviderRepository = serviceProviderRepository;
            _mediator = mediator;
        }
        public async Task DeleteServiceProvider(ServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("ServiceProvider");
            await _serviceProviderRepository.DeleteAsync(serviceProvider);

            //event notification
            await _mediator.EntityDeleted(serviceProvider);
        }
        public async Task<IPagedList<ServiceProvider>> GetServiceProvider(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceProviderRepository.Table;
            return await PagedList<ServiceProvider>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ServiceProvider>> GetPPRSServiceProvider(string keyword,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceProviderRepository.Table;
            query = query.Where(m => m.IsPprs == true);
            if (!String.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(f =>
                   f.Provience.ToLower().Contains(keyword)
                   ||
                   f.NameEnglish.ToLower().Contains(keyword)
                   ||
                   f.NameNepali.ToLower().Contains(keyword)
                   ||
                   f.MobileNo.Contains(keyword)
                   ||
                   f.District.ToLower().Contains(keyword)
                   ||
                   f.LocalLevel.ToLower().Contains(keyword)
               );
            }

            return await PagedList<ServiceProvider>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ServiceProvider>> GetServiceProvider(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceProviderRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<ServiceProvider>.Create(query, pageIndex, pageSize);
        }

        public Task<ServiceProvider> GetServiceProviderById(string id)
        {
            return _serviceProviderRepository.GetByIdAsync(id);
        }

        public async Task InsertServiceProvider(ServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            await _serviceProviderRepository.InsertAsync(serviceProvider);

            //event notification
            await _mediator.EntityInserted(serviceProvider);
        }

        public async Task UpdateServiceProvider(ServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            await _serviceProviderRepository.UpdateAsync(serviceProvider);

            //event notification
            await _mediator.EntityUpdated(serviceProvider);
        }

    }
}
