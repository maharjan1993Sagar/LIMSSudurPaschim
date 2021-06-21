using LIMS.Domain;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public interface IAnimalTypeService
    {
        Task<AnimalType> GetAnimalTypeById(string id);
        Task<List<AnimalType>> GetAnimalTypeBySpeciesId(string id);

        Task<IPagedList<AnimalType>> GetAnimalType(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteAnimalType(AnimalType animalType);

        Task InsertAnimalType(AnimalType animalType);

        Task UpdateAnimalType(AnimalType animalType);
    }
}
