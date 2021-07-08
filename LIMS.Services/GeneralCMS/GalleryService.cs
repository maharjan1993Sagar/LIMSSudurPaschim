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
  public  class GalleryService:IGalleryService
    {
        private readonly IRepository<Gallery> _galleryRepository;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;

       public GalleryService(IRepository<Gallery> galleryRepository, IMediator mediator,IWorkContext workContext)
        {
            _galleryRepository = galleryRepository;
            _mediator = mediator;
            _workContext = workContext;
        }
        public async Task<List<Gallery>> GetAll()
        {
            var gallery = _galleryRepository.Table;
            return gallery.ToList();
        }
        public async Task DeleteGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("Gallery");
            await _galleryRepository.DeleteAsync(gallery);

            //event notification
            await _mediator.EntityDeleted(gallery);
        }

        public async Task<IPagedList<Gallery>> GetGallery(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _galleryRepository.Table;
            return await PagedList<Gallery>.Create(query, pageIndex, pageSize);
        }
        
        public async Task<IPagedList<Gallery>> GetGalleryByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _galleryRepository.Collection;
            var filter = Builders<Gallery>.Filter.Eq("UserId", userId);
            return await PagedList<Gallery>.Create(query,filter, pageIndex, pageSize);
        }
        
        public Task<Gallery> GetGalleryById(string Id)
        {
            return _galleryRepository.GetByIdAsync(Id);
        }
        public async Task InsertGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("Gallery");
            await _galleryRepository.InsertAsync(gallery);

            //event notification
            await _mediator.EntityInserted(gallery);
        }

        public async Task UpdateGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("Gallery");
            await _galleryRepository.UpdateAsync(gallery);

            //event notification
            await _mediator.EntityUpdated(gallery);
        }

        public async Task UpdateGallery(List<Gallery> gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("Gallery");

            await _galleryRepository.UpdateAsync(gallery);          
        }

        #region Farm pictures

        /// <summary>
        /// Deletes a farm picture
        /// </summary>
        /// <param name="farmPicture">Farm picture</param>
        public virtual async Task DeletePicture(NewsEventFile picture)
        {
            if (picture == null)
                throw new ArgumentNullException("Picture");

            var updatebuilder = Builders<Gallery>.Update;
            var update = updatebuilder.Pull(p => p.Images, picture);
            await _galleryRepository.Collection.UpdateOneAsync(new BsonDocument("_id", picture.CMSEntityId), update);

            //event notification
            await _mediator.EntityDeleted(picture);
        }

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="farmPicture">Farm picture</param>
        public virtual async Task InsertPicture(NewsEventFile picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var updatebuilder = Builders<Gallery>.Update;
            var update = updatebuilder.AddToSet(p => p.Images, picture);
            await _galleryRepository.Collection.UpdateOneAsync(new BsonDocument("_id", picture.CMSEntityId), update);

            //event notification
            await _mediator.EntityInserted(picture);
        }

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="picture">Farm picture</param>
        public virtual async Task UpdatePicture(NewsEventFile picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var builder = Builders<Gallery>.Filter;
            var filter = builder.Eq(x => x.Id, picture.CMSEntityId);
            filter = filter & builder.ElemMatch(x => x.Images, y => y.Id == picture.Id);
            var update = Builders<Gallery>.Update
                .Set(x => x.Images.ElementAt(-1).DisplayOrder, picture.DisplayOrder)
                .Set(x => x.Images.ElementAt(-1).MimeType, picture.MimeType)
                .Set(x => x.Images.ElementAt(-1).SeoFilename, picture.SeoFilename)
                .Set(x => x.Images.ElementAt(-1).AltAttribute, picture.AltAttribute)
                .Set(x => x.Images.ElementAt(-1).TitleAttribute, picture.TitleAttribute);

            await _galleryRepository.Collection.UpdateManyAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(picture);
        }
        #endregion


    }


}
