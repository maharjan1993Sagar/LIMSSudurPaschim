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
    public class LivestockBreedService: ILivestockBreedService
    {

        private readonly IRepository<LivestockBreed> _LivestockBreedRepository;
        private readonly IMediator _mediator;
        public LivestockBreedService(IRepository<LivestockBreed> LivestockBreedRepository, IMediator mediator)
        {
            _LivestockBreedRepository = LivestockBreedRepository;
            _mediator = mediator;
        }
        public async Task DeleteBreed(LivestockBreed LivestockBreed)
        {
            if (LivestockBreed == null)
                throw new ArgumentNullException("LivestockBreed");
            await _LivestockBreedRepository.DeleteAsync(LivestockBreed);

            //event notification
            await _mediator.EntityDeleted(LivestockBreed);
        }

        public async Task<IPagedList<LivestockBreed>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _LivestockBreedRepository.Table;
            return await PagedList<LivestockBreed>.Create(query, pageIndex, pageSize);
        }

        public Task<LivestockBreed> GetBreedById(string Id)
        {
            return _LivestockBreedRepository.GetByIdAsync(Id);
        }

        public async Task<List<LivestockBreed>> GetBreedBySpeciesId(string speciesId)
        {
            var filter = Builders<LivestockBreed>.Filter.Eq(x => x.Species.Id, speciesId);
            return _LivestockBreedRepository.Collection.Find(filter).ToList();
        }
        public async Task<List<LivestockBreed>> GetBreedByBreedType(string breedType)
        {
            //  throw new NotImplementedException();
            var filter = Builders<LivestockBreed>.Filter.Eq(x => x.Type, breedType);
            return _LivestockBreedRepository.Collection.Find(filter).ToList();
        }

        public async Task InsertBreed(LivestockBreed LivestockBreed)
        {
            if (LivestockBreed == null)
                throw new ArgumentNullException("LivestockBreed");
            await _LivestockBreedRepository.InsertAsync(LivestockBreed);

            //event notification
            await _mediator.EntityInserted(LivestockBreed);
        }

        public async Task UpdateBreed(LivestockBreed LivestockBreed)
        {
            if (LivestockBreed == null)
                throw new ArgumentNullException("LivestockBreed");
            await _LivestockBreedRepository.UpdateAsync(LivestockBreed);

            //event notification
            await _mediator.EntityUpdated(LivestockBreed);
        }

        public async Task UpdateBreed(List<LivestockBreed> LivestockBreeds)
        {
            if (LivestockBreeds == null)
                throw new ArgumentNullException("LivestockBreed");

            await _LivestockBreedRepository.UpdateAsync(LivestockBreeds);
        }
    }
}

