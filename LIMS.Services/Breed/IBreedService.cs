using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Breed;
namespace LIMS.Services.Breed
{
    public interface IBreedService
    {
        Task<BreedReg> GetBreedById(string Id);

        Task<IPagedList<BreedReg>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteBreed(BreedReg breedReg);

        Task InsertBreed(BreedReg breedReg);

        Task UpdateBreed(BreedReg breedReg);

        Task UpdateBreed(List<BreedReg> breedRegs);

        Task<List<BreedReg>> GetBreedBySpeciesId(string speciesId);
        Task<List<BreedReg>> GetBreedByBreedType(string breedType);
    }
}
