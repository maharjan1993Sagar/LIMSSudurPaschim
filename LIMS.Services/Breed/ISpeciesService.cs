using LIMS.Domain;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public interface ISpeciesService
    {
        Task<Species> GetSpeciesById(string id);

        Task<IPagedList<Species>> GetSpecies(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteSpecies(Species species);

        Task InsertSpecies(Species species);

        Task UpdateSpecies(Species species);
    }
}
