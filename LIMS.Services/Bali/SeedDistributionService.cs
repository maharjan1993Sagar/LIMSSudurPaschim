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
    public class SeedDistributionService:ISeedDistributionService
    {
        private readonly IRepository<SeedDistribution> _SeedDistributionRepository;
        private readonly IMediator _mediator;
        public SeedDistributionService(IRepository<SeedDistribution> SeedDistributionRepository, IMediator mediator)
        {
            _SeedDistributionRepository = SeedDistributionRepository;
            _mediator = mediator;
        }
        public async Task DeleteSeedDistribution(SeedDistribution SeedDistribution)
        {
            if (SeedDistribution == null)
                throw new ArgumentNullException("SeedDistribution");

            await _SeedDistributionRepository.DeleteAsync(SeedDistribution);

            //event notification
            await _mediator.EntityDeleted(SeedDistribution);
        }
        public async Task<IPagedList<SeedDistribution>> GetSeedDistribution(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _SeedDistributionRepository.Table;
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

            return await PagedList<SeedDistribution>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<SeedDistribution>> GetSeedDistribution(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _SeedDistributionRepository.Table;
            if(!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(fiscalyear))
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

            return await PagedList<SeedDistribution>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<SeedDistribution>> GetSeedDistribution(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _SeedDistributionRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<SeedDistribution>.Create(query, pageIndex, pageSize);
        }

        public Task<SeedDistribution> GetSeedDistributionById(string id)
        {
            return _SeedDistributionRepository.GetByIdAsync(id);
        }

        public async Task InsertSeedDistribution(SeedDistribution SeedDistribution)
        {
            if (SeedDistribution == null)
                throw new ArgumentNullException("Livestock");

            await _SeedDistributionRepository.InsertAsync(SeedDistribution);

            //event notification
            await _mediator.EntityInserted(SeedDistribution);
        }

        public Task InsertSeedDistributionList(List<SeedDistribution> SeedDistributions)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSeedDistribution(SeedDistribution SeedDistribution)
        {
            if (SeedDistribution == null)
                throw new ArgumentNullException("baliregister");

            await _SeedDistributionRepository.UpdateAsync(SeedDistribution);

            //event notification
            await _mediator.EntityUpdated(SeedDistribution);
        }

        public Task UpdateSeedDistributionList(List<SeedDistribution> SeedDistributions)
        {
            throw new NotImplementedException();
        }
    }
}
