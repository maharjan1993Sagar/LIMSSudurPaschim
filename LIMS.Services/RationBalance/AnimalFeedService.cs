using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.RationBalance;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.RationBalance
{
    public class AnimalFeedService:IAnimalFeedService
    {
        private readonly IRepository<AnimalFeed> _animalFeedRepository;
        private readonly IMediator _mediator;
        public AnimalFeedService(IRepository<AnimalFeed> animalFeedRepository, IMediator mediator)
        {
            _animalFeedRepository = animalFeedRepository;
            _mediator = mediator;
        }

        public async Task DeleteAnimalFeed(AnimalFeed animalFeed)
        {
            if (animalFeed == null)
                throw new ArgumentNullException("AnimalFeed");

            await _animalFeedRepository.DeleteAsync(animalFeed);

            //event notification
            await _mediator.EntityDeleted(animalFeed);
        }

        public async Task<AnimalFeed> GetAnimalFeedById(string id)
        {
            return await _animalFeedRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<AnimalFeed>> GetFeedLibraries(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalFeedRepository.Table;
            return await PagedList<AnimalFeed>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertAnimalFeed(AnimalFeed animalFeed)
        {
            if (animalFeed == null)
                throw new ArgumentNullException("AnimalFeed");

            await _animalFeedRepository.InsertAsync(animalFeed);

            //event notification
            await _mediator.EntityInserted(animalFeed);
        }

        public async Task InsertAnimalFeedList(IList<AnimalFeed> feedLibraries)
        {
            if (feedLibraries.Count < 1)
                throw new ArgumentNullException("AnimalFeed");
            await _animalFeedRepository.InsertManyAsync(feedLibraries);
        }

        public async Task UpdateAnimalFeed(AnimalFeed animalFeed)
        {
            if (animalFeed == null)
                throw new ArgumentNullException("AnimalFeed");

            await _animalFeedRepository.UpdateAsync(animalFeed);

            //event notification
            await _mediator.EntityUpdated(animalFeed);
        }

        public async Task UpdateAnimalFeedList(IList<AnimalFeed> feedLibraries)
        {
            if (feedLibraries.Count < 1)
                throw new ArgumentNullException("AnimalFeed");
            foreach (var item in feedLibraries)
            {
                await _animalFeedRepository.UpdateAsync(item);
            }
        }
    }
}
