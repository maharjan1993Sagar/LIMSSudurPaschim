using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.SemenDistribution;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Semen
{
    public class SemenDistributionService : ISemenDistributionService
    {
        private IRepository<SemenDistribution> _repository;
        private readonly IMediator _mediator;

        public SemenDistributionService(IRepository<SemenDistribution>  repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
           
    }
        public Task DeleteSemenDistribution(SemenDistribution semenDistribution)
        {
            throw new NotImplementedException();
        }

        public  Task<SemenDistribution> GetSemenDistributionById(string id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<IPagedList<SemenDistribution>> GetSemenDistribution( List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _repository.Table;
            query = query.Where(m =>createdby.Contains(m.CreatedBy));
            return await PagedList<SemenDistribution>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertSemenDIstribution(SemenDistribution semenDistribution)
        {
            if (semenDistribution != null)
            {
                await _repository.InsertAsync(semenDistribution);
                await _mediator.EntityUpdated(semenDistribution);
            }
            else
            {
                throw new ArgumentNullException("semen");
            }
        }

        public async Task InsertSemenDistributionList(List<SemenDistribution> semenDistributions)
        {
            if (semenDistributions.Count < 1)
                throw new ArgumentNullException("semen");
            await _repository.InsertManyAsync(semenDistributions);

        }

        public async Task UpdateSemenDistribution(SemenDistribution semenDistribution)
        {
            if (semenDistribution == null)
                throw new ArgumentNullException("semen");
            var semen = _repository.GetById(semenDistribution.Id);
            if(semen==null)
                throw new ArgumentNullException("semen");
            await _repository.UpdateAsync(semenDistribution);

        }
    }
}
