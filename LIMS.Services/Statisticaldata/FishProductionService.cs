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
    public class FishProductionService:IFishProductionService
    {
        private readonly IRepository<FishProduction> _fishProductionRepository;
        private readonly IMediator _mediator;
        public FishProductionService(IRepository<FishProduction> fishProductionRepository, IMediator mediator)
        {
            _fishProductionRepository = fishProductionRepository;
            _mediator = mediator;
        }
        public async Task DeleteFishProduction(FishProduction fishProduction)
        {
            if (fishProduction == null)
                throw new ArgumentNullException("FishProduction");

            await _fishProductionRepository.DeleteAsync(fishProduction);

            //event notification
            await _mediator.EntityDeleted(fishProduction);
        }

        public async Task<IPagedList<FishProduction>> GetFishProduction(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _fishProductionRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<FishProduction>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<FishProduction>> GetFishProduction(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fishProductionRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));

            query = query.Where(
              m => m.FiscalYear.Id == fiscalyear
            );


            return await PagedList<FishProduction>.Create(query, pageIndex, pageSize);
        }

        public Task<FishProduction> GetFishProductionById(string Id)
        {
            return _fishProductionRepository.GetByIdAsync(Id);

        }

        public async Task InsertFishProduction(FishProduction fishProduction)
        {
            if (fishProduction == null)
                throw new ArgumentNullException("FishProduction");

            await _fishProductionRepository.InsertAsync(fishProduction);

            //event notification
            await _mediator.EntityInserted(fishProduction);
        }

        public async Task UpdateFishProduction(FishProduction fishProduction)
        {
            if (fishProduction == null)
                throw new ArgumentNullException("FishProduction");

            await _fishProductionRepository.UpdateAsync(fishProduction);

            //event notification
            await _mediator.EntityUpdated(fishProduction);
        }
        public async Task InsertFishProductionList(List<FishProduction> fishProductions)
        {
            if (fishProductions.Count < 1)
                throw new ArgumentNullException("FishProduction");
            await _fishProductionRepository.InsertManyAsync(fishProductions);


        }
        public async Task UpdateFishProductionList(List<FishProduction> fishProductions)
        {
            if (fishProductions.Count < 1)
                throw new ArgumentNullException("FishProduction");
            foreach (var item in fishProductions)
            {
                await _fishProductionRepository.UpdateAsync(item);
            }


        }

        public async Task<IPagedList<FishProduction>> GetFilteredFishProduction(string createdBy, string fiscalYearId, string locallevel, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fishProductionRepository.Table;
            if(!String.IsNullOrEmpty(createdBy))
            {
                query = query.Where(m => m.CreatedBy == createdBy);
            }
            if(!String.IsNullOrEmpty(fiscalYearId))
            {
                query = query.Where(m => m.FiscalYearId == fiscalYearId);
            }
            if (!String.IsNullOrEmpty(locallevel))
            {
                query = query.Where(m => m.LocalLevel == locallevel);
            }
          
            return await PagedList<FishProduction>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<FishProduction>> GetFilteredFishProduction(string createdBy, string fiscalYearId, string quater,string natureOfProduction, string localevel = "", string ward = "", string farmid = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fishProductionRepository.Table;
            if (!string.IsNullOrEmpty(createdBy))
            {
                query = query.Where(m => m.CreatedBy == createdBy);
            }
            query = query.Where(m => 
              m.FiscalYear.Id == fiscalYearId &&
              m.NatureOfProduction==natureOfProduction&&
              m.Ward==ward  &&
              m.LocalLevel==localevel
            );
            //if (!string.IsNullOrEmpty(farmid))
            //{
            //    query = query.Where(m => m.FarmId == farmid);
            //}
            //else
            //{
            //    query = query.Where(m => m.LocalLevel == localevel && m.Ward == ward);

            //}
            return await PagedList<FishProduction>.Create(query, pageIndex, pageSize);

        }
    }
}
