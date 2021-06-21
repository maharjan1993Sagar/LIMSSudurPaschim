using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public class SpeciesService: ISpeciesService
    {
        private readonly IRepository<Species> _speciesRepository;
        private readonly IMediator _mediator;
        public SpeciesService(IRepository<Species> speciesRepository, IMediator mediator)
        {
            _speciesRepository = speciesRepository;
            _mediator = mediator;
        }
        public async Task DeleteSpecies(Species species)
        {
            if (species == null)
                throw new ArgumentNullException("Species");
            await _speciesRepository.DeleteAsync(species);

            //event notification
            await _mediator.EntityDeleted(species);
        }

        public async Task<IPagedList<Species>> GetSpecies(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _speciesRepository.Table;
            return await PagedList<Species>.Create(query, pageIndex, pageSize);
        }

        public Task<Species> GetSpeciesById(string id)
        {
            return _speciesRepository.GetByIdAsync(id);
        }

        public async Task InsertSpecies(Species species)
        {
            if (species == null)
                throw new ArgumentNullException("species");
            await _speciesRepository.InsertAsync(species);

            //event notification
            await _mediator.EntityInserted(species);
        }

        public async Task UpdateSpecies(Species species)
        {
            if (species == null)
                throw new ArgumentNullException("species");
            await _speciesRepository.UpdateAsync(species);

            //event notification
            await _mediator.EntityUpdated(species);
        }
    }
}

