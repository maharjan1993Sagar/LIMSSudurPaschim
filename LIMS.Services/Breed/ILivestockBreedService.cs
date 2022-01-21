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
    public interface ILivestockBreedService
    {

        Task<LivestockBreed> GetBreedById(string Id);

        Task<IPagedList<LivestockBreed>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteBreed(LivestockBreed LivestockBreed);

        Task InsertBreed(LivestockBreed LivestockBreed);

        Task UpdateBreed(LivestockBreed LivestockBreed);

        Task UpdateBreed(List<LivestockBreed> LivestockBreeds);

        Task<List<LivestockBreed>> GetBreedBySpeciesId(string speciesId);
        Task<List<LivestockBreed>> GetBreedByBreedType(string breedType);
    }
}
