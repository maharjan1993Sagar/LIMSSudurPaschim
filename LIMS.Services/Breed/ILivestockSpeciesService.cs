using LIMS.Domain;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public interface ILivestockSpeciesService
    {
        
        Task<LivestockSpecies> GetBreedById(string Id);

        Task<IPagedList<LivestockSpecies>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteBreed(LivestockSpecies LivestockSpecies);

        Task InsertBreed(LivestockSpecies LivestockSpecies);

        Task UpdateBreed(LivestockSpecies LivestockSpecies);

        Task UpdateBreed(List<LivestockSpecies> LivestockSpeciess);

       // Task<List<LivestockSpecies>> GetSpeciesByLivestockId(string speciesId);
    }
}
