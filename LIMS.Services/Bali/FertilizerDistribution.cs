using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public class FertilizerDistributionService:IFertilizerDistributionService
    {
        private readonly IRepository<FertilizerDistribution> _FertilizerDistributionRepository;
        private readonly IMediator _mediator;
        public FertilizerDistributionService(IRepository<FertilizerDistribution> FertilizerDistributionRepository, IMediator mediator)
        {
            _FertilizerDistributionRepository = FertilizerDistributionRepository;
            _mediator = mediator;
        }
        public async Task DeleteFertilizerDistribution(FertilizerDistribution FertilizerDistribution)
        {
            if (FertilizerDistribution == null)
                throw new ArgumentNullException("FertilizerDistribution");

            await _FertilizerDistributionRepository.DeleteAsync(FertilizerDistribution);

            //event notification
            await _mediator.EntityDeleted(FertilizerDistribution);
        }
        public async Task<IPagedList<FertilizerDistribution>> GetFertilizerDistribution(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _FertilizerDistributionRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<FertilizerDistribution>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FertilizerDistribution>> GetFertilizerDistribution(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _FertilizerDistributionRepository.Table;
            if(!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalYear);
            }
            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(
                  m => m.District == district
                );
            }
            if (!string.IsNullOrEmpty(locallevel))
            {
                query = query.Where(
                  m => m.LocalLevel == locallevel
                );
            }

            return await PagedList<FertilizerDistribution>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FertilizerDistribution>> GetFertilizerDistribution(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _FertilizerDistributionRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<FertilizerDistribution>.Create(query, pageIndex, pageSize);
        }

        public Task<FertilizerDistribution> GetFertilizerDistributionById(string id)
        {
            return _FertilizerDistributionRepository.GetByIdAsync(id);
        }

        public async Task InsertFertilizerDistribution(FertilizerDistribution FertilizerDistribution)
        {
            if (FertilizerDistribution == null)
                throw new ArgumentNullException("Livestock");

            await _FertilizerDistributionRepository.InsertAsync(FertilizerDistribution);

            //event notification
            await _mediator.EntityInserted(FertilizerDistribution);
        }

        public Task InsertFertilizerDistributionList(List<FertilizerDistribution> FertilizerDistributions)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateFertilizerDistribution(FertilizerDistribution FertilizerDistribution)
        {
            if (FertilizerDistribution == null)
                throw new ArgumentNullException("baliregister");

            await _FertilizerDistributionRepository.UpdateAsync(FertilizerDistribution);

            //event notification
            await _mediator.EntityUpdated(FertilizerDistribution);
        }

        public Task UpdateFertilizerDistributionList(List<FertilizerDistribution> FertilizerDistributions)
        {
            throw new NotImplementedException();
        }
    }
}
