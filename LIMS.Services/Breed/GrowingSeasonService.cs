using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public class GrowingSeasonService: ICropsSeason
    {

        private readonly IRepository<CropsSeason> _GrowingSeasonRepository;
        private readonly IMediator _mediator;
        public GrowingSeasonService(IRepository<CropsSeason> GrowingSeasonRepository, IMediator mediator)
        {
            _GrowingSeasonRepository = GrowingSeasonRepository;
            _mediator = mediator;
        }
        public async Task DeleteBreed(CropsSeason CropsSeason)
        {
            if (CropsSeason == null)
                throw new ArgumentNullException("CropsSeason");
            await _GrowingSeasonRepository.DeleteAsync(CropsSeason);

            //event notification
            await _mediator.EntityDeleted(CropsSeason);
        }

        public async Task<IPagedList<CropsSeason>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _GrowingSeasonRepository.Table;
            return await PagedList<CropsSeason>.Create(query, pageIndex, pageSize);
        }

        public Task<CropsSeason> GetBreedById(string Id)
        {
            return _GrowingSeasonRepository.GetByIdAsync(Id);
        }

        public async Task<List<CropsSeason>> GetBreedBySpeciesId(string speciesId)
        {
            var filter = Builders<CropsSeason>.Filter.Eq(x => x.Species.Id, speciesId);
            return _GrowingSeasonRepository.Collection.Find(filter).ToList();
        }
      

        public async Task InsertBreed(CropsSeason CropsSeason)
        {
            if (CropsSeason == null)
                throw new ArgumentNullException("CropsSeason");
            await _GrowingSeasonRepository.InsertAsync(CropsSeason);

            //event notification
            await _mediator.EntityInserted(CropsSeason);
        }

        public async Task UpdateBreed(CropsSeason CropsSeason)
        {
            if (CropsSeason == null)
                throw new ArgumentNullException("CropsSeason");
            await _GrowingSeasonRepository.UpdateAsync(CropsSeason);

            //event notification
            await _mediator.EntityUpdated(CropsSeason);
        }

        public async Task UpdateBreed(List<CropsSeason> GrowingSeasons)
        {
            if (GrowingSeasons == null)
                throw new ArgumentNullException("CropsSeason");

            await _GrowingSeasonRepository.UpdateAsync(GrowingSeasons);
        }

    }
}
