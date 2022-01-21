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
    public class AnimalTypeService:IAnimalTypeService
    {
        private readonly IRepository<AnimalType> _animalTypeRepository;
        private readonly IMediator _mediator;
        public AnimalTypeService(IRepository<AnimalType> animalTypeRepository, IMediator mediator)
        {
            _animalTypeRepository = animalTypeRepository;
            _mediator = mediator;
        }
        public async Task DeleteAnimalType(AnimalType animalType)
        {
            if (animalType == null)
                throw new ArgumentNullException("AnimalType");
            await _animalTypeRepository.DeleteAsync(animalType);

            //event notification
            await _mediator.EntityDeleted(animalType);
        }

        public async Task<IPagedList<AnimalType>> GetAnimalType(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalTypeRepository.Table;
            return await PagedList<AnimalType>.Create(query, pageIndex, pageSize);
        }

        public Task<AnimalType> GetAnimalTypeById(string id)
        {
            return _animalTypeRepository.GetByIdAsync(id);
        }
        public async Task<List<AnimalType>> GetAnimalTypeBySpeciesId(string id)
        {
            var filter = Builders<AnimalType>.Filter.Eq(x => x.SpeciesId, id);
            return  _animalTypeRepository.Collection.Find(filter).ToList();
        }

        public async Task InsertAnimalType(AnimalType animalType)
        {
            if (animalType == null)
                throw new ArgumentNullException("animalType");
            await _animalTypeRepository.InsertAsync(animalType);

            //event notification
            await _mediator.EntityInserted(animalType);
        }

        public async Task UpdateAnimalType(AnimalType animalType)
        {
            if (animalType == null)
                throw new ArgumentNullException("animalType");
            await _animalTypeRepository.UpdateAsync(animalType);

            //event notification
            await _mediator.EntityUpdated(animalType);
        }

    }
}
