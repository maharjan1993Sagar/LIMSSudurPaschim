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
   public class ProductionDataService: IProductionionDataService
    {
        private readonly IRepository<Production> _productionRepository;
        private readonly IMediator _mediator;
        public ProductionDataService(IRepository<Production> productionRepository, IMediator mediator)
        {
            _productionRepository = productionRepository;
            _mediator = mediator;
        }
        public async Task DeleteProduction(Production production)
        {
            if (production == null)
                throw new ArgumentNullException("Production");

            await _productionRepository.DeleteAsync(production);

            //event notification
            await _mediator.EntityDeleted(production);
        }

        public async Task<IPagedList<Production>> GetProduction(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;


            return await PagedList<Production>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Production>> GetProduction(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);


            return await PagedList<Production>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Production>> GetProductionByKeyword(string createdby, string keyword, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.ProductionType.ToLower().Contains(keyword.ToLower())
                  ||
                  m.LocalLevel.ToLower().Contains(keyword.ToLower())
                ||
                 m.District.ToLower().Contains(keyword.ToLower())
                 ||
                                   m.Ward.Contains(keyword)

                );
            }


            return await PagedList<Production>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Production>> GetProduction(List<string> createdby, string type,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;
            query = query.Where(m =>createdby.Contains(m.CreatedBy)&&m.ProductionType.ToLower()==type);


            return await PagedList<Production>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Production>> GetProduction(string createdby,string fiscalyear,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;

            query = query.Where(m => m.CreatedBy == createdby&& m.FiscalYear.Id==fiscalyear);
            return await PagedList<Production>.Create(query, pageIndex, pageSize);
        }


        public Task<Production> GetProductionById(string Id)
        {
            return _productionRepository.GetByIdAsync(Id);

        }

        public async Task InsertProduction(Production production)
        {
            if (production == null)
                throw new ArgumentNullException("Production");

            await _productionRepository.InsertAsync(production);

            //event notification
            await _mediator.EntityInserted(production);
        }

        public async Task UpdateProduction(Production production)
        {
            if (production == null)
                throw new ArgumentNullException("Production");

            await _productionRepository.UpdateAsync(production);

            //event notification
            await _mediator.EntityUpdated(production);
        }
        public async Task InsertProductionList(List<Production> productions)
        {
            if (productions.Count < 1)
                throw new ArgumentNullException("Production");
            await _productionRepository.InsertManyAsync(productions);


        }
        public async Task UpdateProductionList(List<Production> productions)
        {
            if (productions.Count < 1)
                throw new ArgumentNullException("Production");
            foreach (var item in productions)
            {
                await _productionRepository.UpdateAsync(item);
            }


        }
        public async Task<IPagedList<Production>> GetFilteredProduction(string createdby, string type, string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;
            if (createdby != "molmac")
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (fiscalYearId != null && (type == null && string.IsNullOrEmpty(district)))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId);

            }
            if (fiscalYearId != null && type != null && string.IsNullOrEmpty(district))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.ProductionType == type);

            }
            if (fiscalYearId != null && type != null && !string.IsNullOrEmpty(district) && string.IsNullOrEmpty(LocalLevel))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.ProductionType == type && m.District == district);

            }
            
            if (fiscalYearId != null && type != null && !string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(LocalLevel))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.ProductionType == type && m.LocalLevel == LocalLevel);

            }
            return await PagedList<Production>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<Production>> GetFilteredProduction(string fiscalyearId,string productiontype,string createdBy,string district,string locallevel,string ward, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productionRepository.Table;
            query = query.Where(m =>
              m.LocalLevel == locallevel &&
              m.District==district&&
              m.ProductionType == productiontype &&
              m.FiscalYear.Id==fiscalyearId &&
              m.Ward==ward&&
              m.CreatedBy == createdBy
            );
            return await PagedList<Production>.Create(query, pageIndex, pageSize);
        }
    }
}
