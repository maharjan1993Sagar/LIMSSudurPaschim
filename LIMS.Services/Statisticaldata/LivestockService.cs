using LIMS.Domain;
using LIMS.Domain.Breed;
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
        public async Task<IPagedList<Livestock>> GetLivestock( int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _livestockRepository.Table;


            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Livestock>> GetLivestock(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear="")
        {
            var query = _livestockRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }


            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Livestock>> GetLivestock(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _livestockRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id==fiscalyear);

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

        public async Task<IPagedList<Livestock>> GetFilteredLivestock(string createdby, string speciesId, string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockRepository.Table;
            if (!String.IsNullOrEmpty(createdby) )
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(fiscalYearId))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId);
            }
            if (!String.IsNullOrEmpty(LocalLevel))
            {
                query = query.Where(m => m.LocalLevel == LocalLevel);
            }
            //if (createdby != "molmac")
            //{
            //    query = query.Where(m => m.CreatedBy == createdby);
            //}
            //if(fiscalYearId!=null&&(speciesId==null&&string.IsNullOrEmpty(district)))
            //{
            //    query = query.Where(m => m.FiscalYear.Id==fiscalYearId);

            //}
            //if (fiscalYearId != null && !string.IsNullOrEmpty(speciesId) && string.IsNullOrEmpty(district))
            //{
            //    query = query.Where(m => m.FiscalYear.Id == fiscalYearId&&m.Species.Id==speciesId);

            //}
            //if (fiscalYearId != null && !string.IsNullOrEmpty(speciesId) && !string.IsNullOrEmpty(district) && string.IsNullOrEmpty(LocalLevel) )
            //{
            //    query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.Species.Id == speciesId&&m.District==district);

            //}
            //if (fiscalYearId != null && !string.IsNullOrEmpty(speciesId) && district != null && !string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(LocalLevel))
            //{
            //    query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.Species.Id == speciesId && m.LocalLevel == LocalLevel);

            //}
            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);
        
        }
        //used for filtering
        public async Task<IPagedList<Livestock>> GetFilteredLivestock(string createdBy, string speciesId, string district, string fiscalYearId, string quater,string locallevel,string month, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockRepository.Table;
            if (createdBy != "molmac")
            {
                query = query.Where(m => m.CreatedBy == createdBy);
            }
            if (fiscalYearId != null && (speciesId == null && string.IsNullOrEmpty(district)))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId);

            }
            if (fiscalYearId != null && speciesId != null && string.IsNullOrEmpty(district))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.Species.Id == speciesId);

            }
            if (fiscalYearId != null && speciesId != null && !string.IsNullOrEmpty(district) && string.IsNullOrEmpty(locallevel))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.Species.Id == speciesId && m.District == district);

            }
            if (fiscalYearId != null && speciesId != null && district != null && !string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(locallevel) && string.IsNullOrEmpty(month))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.Species.Id == speciesId && m.LocalLevel == locallevel);

            }
            if (fiscalYearId != null && speciesId != null && district != null && !string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(locallevel) && !string.IsNullOrEmpty(month
                ))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.Species.Id == speciesId && m.LocalLevel == locallevel && m.Ward == month);

            }

                return await PagedList<Livestock>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<Livestock>> GetFilteredsLivestock(string createdby,  string fiscalYearId, string LocalLevel, string district,string ward, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _livestockRepository.Table;

            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }


            query = query.Where(m => m.FiscalYear.Id == fiscalYearId &&  m.LocalLevel == LocalLevel&&m.Ward==ward);

          
            return await PagedList<Livestock>.Create(query, pageIndex, pageSize);

        }
    }
}
