using LIMS.Core;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;
using LIMS.Domain.NewsEvent;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.GeneralCMS
{
  public class BannerService:IBannerService
    {
        private readonly IRepository<Banner> _bannerRepository;
        private readonly IWorkContext _workContext;
        private readonly IMediator _mediator;

       public BannerService(IRepository<Banner> bannerRepository, IMediator mediator,IWorkContext context)
        {
            _bannerRepository = bannerRepository;
            _mediator = mediator;
            _workContext = context;
        }
        public async Task<List<Banner>> GetAll()
        {
            var banners =  _bannerRepository.Table;
            return banners.ToList();
        }
        public async Task DeleteBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("Banner");
            await _bannerRepository.DeleteAsync(banner);

            //event notification
            await _mediator.EntityDeleted(banner);
        }

        public async Task<IPagedList<Banner>> GetBanner(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _bannerRepository.Table;
            return await PagedList<Banner>.Create(query, pageIndex, pageSize);
        }
        
        public async Task<IPagedList<Banner>> GetBannerByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        
        {
            var table = _bannerRepository.Table;
            var pagelst= await PagedList<Banner>.Create(table, pageIndex, pageSize);
            var userId = _workContext.CurrentCustomer.Id;
            var query = _bannerRepository.Collection;
            var filter = Builders<Banner>.Filter.Eq("UserId", userId);
            return await PagedList<Banner>.Create(query,filter, pageIndex, pageSize);
        }
        
        public Task<Banner> GetBannerById(string Id)
        {
            return _bannerRepository.GetByIdAsync(Id);
        }
        public async Task InsertBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("Banner");
            await _bannerRepository.InsertAsync(banner);

            //event notification
            await _mediator.EntityInserted(banner);
        }

        public async Task UpdateBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("Banner");
            await _bannerRepository.UpdateAsync(banner);

            //event notification
            await _mediator.EntityUpdated(banner);
        }

      

    }


}
