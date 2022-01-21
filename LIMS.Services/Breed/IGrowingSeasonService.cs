using LIMS.Domain;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public interface ICropsSeason
    {
        Task<CropsSeason> GetBreedById(string Id);

        Task<IPagedList<CropsSeason>> GetBreed(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteBreed(CropsSeason CropsSeason);

        Task InsertBreed(CropsSeason CropsSeason);

        Task UpdateBreed(CropsSeason CropsSeason);

        Task UpdateBreed(List<CropsSeason> CropsSeasons);

        Task<List<CropsSeason>> GetBreedBySpeciesId(string speciesId);

    }
}
