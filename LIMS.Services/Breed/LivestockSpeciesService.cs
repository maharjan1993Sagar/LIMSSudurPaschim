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
    public class LivestockSpeciesService:ILivestockSpeciesService
    {

        private readonly IRepository<LivestockSpecies> _LivestockSpeciesRepository;
        private readonly IMediator _mediator;
        public LivestockSpeciesService(IRepository<LivestockSpecies> LivestockSpeciesRepository, IMediator mediator)
        {
            _LivestockSpeciesRepository = LivestockSpeciesRepository;
            _mediator = mediator;
        }
        public async Task DeleteBreed(LivestockSpecies LivestockSpecies)
        {
            if (LivestockSpecies == null)
                throw new ArgumentNullException("LivestockSpecies");
            await _LivestockSpeciesRepository.DeleteAsync(LivestockSpecies);

            //event notification
            await _mediator.EntityDeleted(LivestockSpecies);
        }

        public async Task<IPagedList<LivestockSpecies>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _LivestockSpeciesRepository.Table;
            return await PagedList<LivestockSpecies>.Create(query, pageIndex, pageSize);
        }

        public Task<LivestockSpecies> GetBreedById(string Id)
        {
            return _LivestockSpeciesRepository.GetByIdAsync(Id);
        }

        //public async Task<List<LivestockSpecies>> GetSpeciesByLivestockId(string speciesId)
        //{
        //    var filter = Builders<LivestockSpecies>.Filter.Eq(x => x.Livestock.Id, speciesId);
        //    return _LivestockSpeciesRepository.Collection.Find(filter).ToList();
        //}
   
        public async Task InsertBreed(LivestockSpecies LivestockSpecies)
        {
            if (LivestockSpecies == null)
                throw new ArgumentNullException("LivestockSpecies");
            await _LivestockSpeciesRepository.InsertAsync(LivestockSpecies);

            //event notification
            await _mediator.EntityInserted(LivestockSpecies);
        }

        public async Task UpdateBreed(LivestockSpecies LivestockSpecies)
        {
            if (LivestockSpecies == null)
                throw new ArgumentNullException("LivestockSpecies");
            await _LivestockSpeciesRepository.UpdateAsync(LivestockSpecies);

            //event notification
            await _mediator.EntityUpdated(LivestockSpecies);
        }

        public async Task UpdateBreed(List<LivestockSpecies> LivestockSpeciess)
        {
            if (LivestockSpeciess == null)
                throw new ArgumentNullException("LivestockSpecies");

            await _LivestockSpeciesRepository.UpdateAsync(LivestockSpeciess);
        }
    }
}
