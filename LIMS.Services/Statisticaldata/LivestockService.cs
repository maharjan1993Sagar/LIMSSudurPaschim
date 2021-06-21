using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.StatisticalData;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Statisticaldata
{
    public class LivestockService:ILivestockService
    {
        private readonly IRepository<Livestock> _livestockRepository;
        private readonly IMediator _mediator;
        public LivestockService(IRepository<Livestock> livestockRepository, IMediator mediator)
        {
            _livestockRepository = livestockRepository;
            _mediator = mediator;
        }
        public async Task DeleteLivestock(Livestock livestock)
        {
            if (livestock == null)
                throw new ArgumentNullException("Livestock");

            await _livestockRepository.DeleteAsync(livestock);

            //event notification
            await _mediator.EntityDeleted(livestock);
        }

        public async Task<IPagedList<Livestock>> GetLivestock(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear="")
        {
            var query = _livestockRepository.Table;
            query=query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m=> m.FiscalYear.Id==fiscalyear
                );
            }
           
            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);
        }

        public Task<Livestock> GetLivestockById(string Id)
        {
            return _livestockRepository.GetByIdAsync(Id);

        }

        public async Task InsertLivestock(Livestock livestock)
        {
            if (livestock == null)
                throw new ArgumentNullException("Livestock");

            await _livestockRepository.InsertAsync(livestock);

            //event notification
            await _mediator.EntityInserted(livestock);
        }

        public async Task UpdateLivestock(Livestock livestock)
        {
            if (livestock == null)
                throw new ArgumentNullException("Livestock");

            await _livestockRepository.UpdateAsync(livestock);

            //event notification
            await _mediator.EntityUpdated(livestock);
        }
        public async Task InsertLivestockList(List<Livestock> livestocks)
        {
            if (livestocks.Count<1)
                throw new ArgumentNullException("Livestock");
           await _livestockRepository.InsertManyAsync(livestocks);
            

        }
        public async Task UpdateLivestockList(List<Livestock> livestocks)
        {
            if (livestocks.Count < 1)
                throw new ArgumentNullException("Livestock");
            foreach(var item in livestocks)
            {
                await _livestockRepository.UpdateAsync(item);
            }


        }

        public async Task<IPagedList<Livestock>> GetFilteredLivestock(string createdBy, string speciesId, string breedType, string fiscalYearId, string quater,string month, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockRepository.Table;
            query = query.Where(m =>
             m.Species.Id==speciesId&&
              m.Quater == quater &&
              m.BreedType == breedType &&
              m.FiscalYear.Id==fiscalYearId &&
              m.CreatedBy == createdBy&&
              m.Month==month
            );
            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);
        
        }
        public async Task<IPagedList<Livestock>> GetFilteredLivestock(string createdBy, string speciesId, string breedType, string fiscalYearId, string quater,string ward,string month, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockRepository.Table;
            query = query.Where(m =>
             m.Species.Id == speciesId &&
              m.Quater == quater &&
              m.BreedType == breedType &&
              m.FiscalYear.Id == fiscalYearId &&
              m.CreatedBy == createdBy &&
              m.Ward==ward &&
              m.Month==month
            );
            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);

        }
    }
}
