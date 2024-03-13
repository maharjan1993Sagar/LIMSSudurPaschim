using LIMS.Core.Caching;
using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public class FarmService : IFarmService
    {
        #region Fields
        private readonly IRepository<Farm> _farmRepository;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public FarmService(IRepository<Farm> farmRepository, IMediator mediator, ICacheManager cacheManager)
        {
            _farmRepository = farmRepository;
            _mediator = mediator;
            _cacheManager = cacheManager;
        }
        #endregion

        public async Task DeleteFarm(Farm farm)
        {
            if (farm == null)
                throw new ArgumentNullException("Farm");
            await _farmRepository.DeleteAsync(farm);

            //event notification
            await _mediator.EntityDeleted(farm);
        }

        public async Task<IPagedList<Farm>> SearchFarm(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _farmRepository.Table;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(f =>
                    f.Category.ToLower().Contains(keyword)
                    ||
                    f.NameEnglish.ToLower().Contains(keyword)
                    ||
                    f.NameNepali.ToLower().Contains(keyword)
                    ||
                    f.Phone.Contains(keyword)
                    ||
                    f.District.ToLower().Contains(keyword)
                );
            }
            return await PagedList<Farm>.Create(query, pageIndex, pageSize);
        }


        public async Task<IPagedList<Farm>> GetFarmByLssId(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _farmRepository.Table;

            if(customerid!=null|| customerid.Count==0)
            {
                query = query.Where(t => customerid.Contains(t.CreatedBy));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(f =>
                    f.Category.ToLower().Contains(keyword)
                    ||
                    f.NameEnglish.ToLower().Contains(keyword)
                    ||
                    f.NameNepali.ToLower().Contains(keyword)
                    ||
                    f.PanNO.ToLower().Contains(keyword)
                    ||
                    f.CitizenshipNo.ToLower().Contains(keyword)
                    ||
                    f.District.ToLower().Contains(keyword)
                    || 
                    f.LocalLevel.ToLower().Contains(keyword)
                );
            }
            return await PagedList<Farm>.Create(query, pageIndex, pageSize);
        }
        public int GetFarmCountByLssId(List<string> customerid)
        {
            var query = _farmRepository.Table.Where(t => customerid.Contains(t.CreatedBy));

            return query.Count();
        }


        public Task<Farm> GetFarmById(string id)
        {
            return _farmRepository.GetByIdAsync(id);
        }
        public Task<PagedList<Farm>> GetFarmByCreatedBy(string createdBy, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _farmRepository.Table;
            query = query.Where(f =>
                    f.CreatedBy==createdBy 
                );
            
            return  PagedList<Farm>.Create(query, pageIndex, pageSize);

        }
        public Task<PagedList<Farm>> GetPPRsFram(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var query = _farmRepository.Table;
            query = query.Where(f =>
                    f.PPRS == true
                );
            if(!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(f =>
                    f.Category.ToLower().Contains(keyword)
                    ||
                    f.NameEnglish.ToLower().Contains(keyword)
                    ||
                    f.NameNepali.ToLower().Contains(keyword)
                    ||
                    f.Phone.Contains(keyword)
                    ||
                    f.District.ToLower().Contains(keyword)
                );
            }
            return PagedList<Farm>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertFarm(Farm farm)
        {
            if (farm == null)
                throw new ArgumentNullException("Farm");
            await _farmRepository.InsertAsync(farm);

            //event notification
            await _mediator.EntityInserted(farm);
        }

        public async Task UpdateFarm(Farm farm)
        {
            if (farm == null)
                throw new ArgumentNullException("Farm");
            await _farmRepository.UpdateAsync(farm);

            //event notification
            await _mediator.EntityUpdated(farm);
        }
        
        #region Farm pictures

        /// <summary>
        /// Deletes a farm picture
        /// </summary>
        /// <param name="farmPicture">Farm picture</param>
        public virtual async Task DeleteFarmPicture(FarmPicture farmPicture)
        {
            if (farmPicture == null)
                throw new ArgumentNullException("farmPicture");

            var updatebuilder = Builders<Farm>.Update;
            var update = updatebuilder.Pull(p => p.FarmPictures, farmPicture);
            await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmPicture.FarmId), update);

            //event notification
            await _mediator.EntityDeleted(farmPicture);
        }

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="farmPicture">Farm picture</param>
        public virtual async Task InsertFarmPicture(FarmPicture farmPicture)
        {
            if (farmPicture == null)
                throw new ArgumentNullException("farmPicture");

            var updatebuilder = Builders<Farm>.Update;
            var update = updatebuilder.AddToSet(p => p.FarmPictures, farmPicture);
            await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmPicture.FarmId), update);

            //event notification
            await _mediator.EntityInserted(farmPicture);
        }

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="farmPicture">Farm picture</param>
        public virtual async Task UpdateFarmPicture(FarmPicture farmPicture)
        {
            if (farmPicture == null)
                throw new ArgumentNullException("farmPicture");

            var builder = Builders<Farm>.Filter;
            var filter = builder.Eq(x => x.Id, farmPicture.FarmId);
            filter = filter & builder.ElemMatch(x => x.FarmPictures, y => y.Id == farmPicture.Id);
            var update = Builders<Farm>.Update
                .Set(x => x.FarmPictures.ElementAt(-1).DisplayOrder, farmPicture.DisplayOrder)
                .Set(x => x.FarmPictures.ElementAt(-1).MimeType, farmPicture.MimeType)
                .Set(x => x.FarmPictures.ElementAt(-1).SeoFilename, farmPicture.SeoFilename)
                .Set(x => x.FarmPictures.ElementAt(-1).AltAttribute, farmPicture.AltAttribute)
                .Set(x => x.FarmPictures.ElementAt(-1).TitleAttribute, farmPicture.TitleAttribute);

            await _farmRepository.Collection.UpdateManyAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(farmPicture);
        }
        #endregion

        #region Farm Shed

        /// <summary>
        /// Deletes a farm shed
        /// </summary>
        /// <param name="farmshed">Farm shed</param>
        public virtual async Task DeleteFarmShed(FarmShed farmShed)
        {
            if (farmShed == null)
                throw new ArgumentNullException("farmShed");

            var updatebuilder = Builders<Farm>.Update;
            var update = updatebuilder.Pull(p => p.FarmSheds, farmShed);
            await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmShed.FarmId), update);

            //event notification
            await _mediator.EntityDeleted(farmShed);
        }

        /// <summary>
        /// Inserts a farm shed
        /// </summary>
        /// <param name="farmShed">Farm shed</param>
        public virtual async Task InsertFarmShed(FarmShed farmShed)
        {
            if (farmShed == null)
                throw new ArgumentNullException("farmShed");

            var updatebuilder = Builders<Farm>.Update;
            var update = updatebuilder.AddToSet(p => p.FarmSheds, farmShed);
            await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmShed.FarmId), update);

            //event notification
            await _mediator.EntityInserted(farmShed);
        }

        /// <summary>
        /// Updates a product shed
        /// </summary>
        /// <param name="farmshed">Farm shed</param>
        public virtual async Task UpdateFarmShed(FarmShed farmShed)
        {
            if (farmShed == null)
                throw new ArgumentNullException("farmShed");

            var builder = Builders<Farm>.Filter;
            var filter = builder.Eq(x => x.Id, farmShed.FarmId);
            filter = filter & builder.ElemMatch(x => x.FarmSheds, y => y.Id == farmShed.Id);
            var update = Builders<Farm>.Update
                .Set(x => x.FarmSheds.ElementAt(-1).Length, farmShed.Length)
                .Set(x => x.FarmSheds.ElementAt(-1).Bredth, farmShed.Bredth)
                .Set(x => x.FarmSheds.ElementAt(-1).Volume, farmShed.Volume)
                .Set(x => x.FarmSheds.ElementAt(-1).Type, farmShed.Type)
               .Set(x => x.FarmSheds.ElementAt(-1).ConstructedDate, farmShed.ConstructedDate)
               .Set(x => x.FarmSheds.ElementAt(-1).Height, farmShed.Height);
            await _farmRepository.Collection.UpdateManyAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(farmShed);
        }
        #endregion

        #region Farm Grass

        /// <summary>
        /// Deletes a farm Grass
        /// </summary>
        /// <param name="farmGrass">Farm Grass</param>
        public virtual async Task DeleteFarmGrass(FarmGrass farmGrass)
        {
            if (farmGrass == null)
                throw new ArgumentNullException("farmGrass");

            var updatebuilder = Builders<Farm>.Update;
            var update = updatebuilder.Pull(p => p.FarmGrasses, farmGrass);
            await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmGrass.FarmId), update);

            //event notification
            await _mediator.EntityDeleted(farmGrass);
        }

        /// <summary>
        /// Inserts a farm Grass
        /// </summary>
        /// <param name="farmGrass">Farm Grass</param>
        public virtual async Task InsertFarmGrass(FarmGrass farmGrass)
        {
            if (farmGrass == null)
                throw new ArgumentNullException("farmGrass");

            var updatebuilder = Builders<Farm>.Update;
            var update = updatebuilder.AddToSet(p => p.FarmGrasses, farmGrass);
            await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmGrass.FarmId), update);

            //event notification
            await _mediator.EntityInserted(farmGrass);
        }

        /// <summary>
        /// Updates a product Grass
        /// </summary>
        /// <param name="farmGrass">Farm Grass</param>
        public virtual async Task UpdateFarmGrass(FarmGrass farmGrass)
        {
            if (farmGrass == null)
                throw new ArgumentNullException("farmGrass");

            var builder = Builders<Farm>.Filter;
            var filter = builder.Eq(x => x.Id, farmGrass.FarmId);
            filter = filter & builder.ElemMatch(x => x.FarmGrasses, y => y.Id == farmGrass.Id);
            var update = Builders<Farm>.Update
                .Set(x => x.FarmGrasses.ElementAt(-1).Type, farmGrass.Type)
                .Set(x => x.FarmGrasses.ElementAt(-1).GrassName, farmGrass.GrassName)
                .Set(x => x.FarmGrasses.ElementAt(-1).Season, farmGrass.Season)
                .Set(x => x.FarmGrasses.ElementAt(-1).TotalArea, farmGrass.TotalArea);
                

            await _farmRepository.Collection.UpdateManyAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(farmGrass);
        }
        #endregion


    }
}
