using LIMS.Domain;
using LIMS.Domain.NewsEvent;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.NewsEvent
{
    public class NewsEventService : INewsEventService
    {
        private readonly IRepository<NewsEventTender> _newsRepository;
        private readonly IMediator _mediator;
        public NewsEventService(IRepository<NewsEventTender> newsRepository, IMediator mediator)
        {
            _newsRepository = newsRepository;
            _mediator = mediator;
        }
        public async Task DeleteNewsEvent(NewsEventTender news)
        {
            if (news == null)
                throw new ArgumentNullException("BreedReg");

            await _newsRepository.DeleteAsync(news);

            //event notification
            await _mediator.EntityDeleted(news);
        }

        public async Task<IPagedList<NewsEventTender>> GetNewsEvent(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _newsRepository.Table;
            return await PagedList<NewsEventTender>.Create(query, pageIndex, pageSize);
        }

        public Task<NewsEventTender> GetNewsEventById(string Id)
        {
            return _newsRepository.GetByIdAsync(Id);
        }



        public async Task InsertNewsEvent(NewsEventTender news)
        {
            if (news == null)
                throw new ArgumentNullException("NewsEventTender");
            await _newsRepository.InsertAsync(news);

            //event notification
            //   await _mediator.EntityInserted(breedReg);
        }

        public async Task UpdateNewsEvent(NewsEventTender news)
        {
            if (news == null)
                throw new ArgumentNullException("NewsEventTender");
            await _newsRepository.UpdateAsync(news);

            //event notification
            //await _mediator.EntityUpdated(breedReg);
        }

        public async Task UpdateNewsEvent(List<NewsEventTender> news)
        {
            if (news == null)
                throw new ArgumentNullException("NewsEventTender");

            await _newsRepository.UpdateAsync(news);
        }

        //public async Task UpdateNewsEvent(NewsEventFile newsFile)
        //{
        //    if (newsFile == null)
        //        throw new ArgumentNullException("NewsFile");

        //    var updatebuilder = Builders<NewsEventTender>.Update;
        //    var update = updatebuilder.AddToSet(p => p.NewsEventFile, newsFile);
        //    await _farmRepository.Collection.UpdateOneAsync(new BsonDocument("_id", farmPicture.FarmId), update);

        //    //event notification
        //    await _mediator.EntityInserted(farmPicture);
        //}


    }


}
