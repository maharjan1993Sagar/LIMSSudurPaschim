using LIMS.Domain;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.User
{
   public interface IServiceProviderService
    {
        Task<ServiceProvider> GetServiceProviderById(string id);
        Task<IPagedList<ServiceProvider>> GetServiceProvider( int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ServiceProvider>> GetPPRSServiceProvider(string keyword,int pageIndex = 0, int pageSize = int.MaxValue);


        Task<IPagedList<ServiceProvider>> GetServiceProvider(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteServiceProvider(ServiceProvider serviceProvider);

        Task InsertServiceProvider(ServiceProvider serviceProvider);

        Task UpdateServiceProvider(ServiceProvider serviceProvider);
    }
}
