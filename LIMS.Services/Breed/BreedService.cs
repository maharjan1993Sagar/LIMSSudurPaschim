using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
  public   class BreedService:IBreedService
    {
        private readonly IRepository<BreedReg> _breedRegRepository;
        private readonly IMediator _mediator;
        public BreedService(IRepository<BreedReg> breedRegRepository, IMediator mediator)
        {
            _breedRegRepository = breedRegRepository;
            _mediator = mediator;
        }
        public async Task DeleteBreed(BreedReg breedReg)
        {
            if (breedReg == null)
                throw new ArgumentNullException("BreedReg");
            await _breedRegRepository.DeleteAsync(breedReg);

            //event notification
            await _mediator.EntityDeleted(breedReg);
        }

        public async Task<IPagedList<BreedReg>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _breedRegRepository.Table;
            return await PagedList<BreedReg>.Create(query, pageIndex, pageSize);
        }

        public Task<BreedReg> GetBreedById(string Id)
        {
            return _breedRegRepository.GetByIdAsync(Id);
        }

        public async Task<List<BreedReg>> GetBreedBySpeciesId(string speciesId)
        {
            var filter = Builders<BreedReg>.Filter.Eq(x => x.Species.Id, speciesId);
            return  _breedRegRepository.Collection.Find(filter).ToList();
        }
        public async Task<List<BreedReg>> GetBreedByBreedType(string breedType)
        {
            var filter = Builders<BreedReg>.Filter.Eq(x => x.Type, breedType);
            return  _breedRegRepository.Collection.Find(filter).ToList();
        }

        public async Task InsertBreed(BreedReg breedReg)
        {
            if (breedReg == null)
                throw new ArgumentNullException("BreedReg");
            await _breedRegRepository.InsertAsync(breedReg);

            //event notification
            await _mediator.EntityInserted(breedReg);
        }

        public async Task UpdateBreed(BreedReg breedReg)
        {
            if (breedReg == null)
                throw new ArgumentNullException("BreedReg");
            await _breedRegRepository.UpdateAsync(breedReg);

            //event notification
            await _mediator.EntityUpdated(breedReg);
        }

        public async Task UpdateBreed(List<BreedReg> breedRegs)
        {
            if (breedRegs == null)
                throw new ArgumentNullException("BreedReg");

            await _breedRegRepository.UpdateAsync(breedRegs);          
        }
    }


}
