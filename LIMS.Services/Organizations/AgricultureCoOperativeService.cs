using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public class AgricultureCoOperativeService : IAgricultureCoOperativeService
    {
        private readonly IRepository<AgricultureCoOperative> _AgricultureCoOperativeRepository;
        private readonly IMediator _mediator;
        public AgricultureCoOperativeService(IRepository<AgricultureCoOperative> AgricultureCoOperativeRepository, IMediator mediator)
        {
            _AgricultureCoOperativeRepository = AgricultureCoOperativeRepository;
            _mediator = mediator;
        }
        public async Task DeleteAgricultureCoOperative(AgricultureCoOperative AgricultureCoOperative)
        {
            if (AgricultureCoOperative == null)
                throw new ArgumentNullException("AgricultureCoOperative");
            await _AgricultureCoOperativeRepository.DeleteAsync(AgricultureCoOperative);

            //event notification
            await _mediator.EntityDeleted(AgricultureCoOperative);
        }

        public async Task<IPagedList<AgricultureCoOperative>> GetAgricultureCoOperative(string createdby, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _AgricultureCoOperativeRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<AgricultureCoOperative>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<AgricultureCoOperative>> GetAgricultureCoOperative(List<string> createdby, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _AgricultureCoOperativeRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<AgricultureCoOperative>.Create(query, pageIndex, pageSize);
        }

        public Task<AgricultureCoOperative> GetAgricultureCoOperativeById(string id)
        {
            return _AgricultureCoOperativeRepository.GetByIdAsync(id);
        }

        public async Task InsertAgricultureCoOperative(AgricultureCoOperative AgricultureCoOperative)
        {
            if (AgricultureCoOperative == null)
                throw new ArgumentNullException("AgricultureCoOperative");
            await _AgricultureCoOperativeRepository.InsertAsync(AgricultureCoOperative);

            //event notification
            await _mediator.EntityInserted(AgricultureCoOperative);
        }
        public async Task InsertAgricultureCoOperativeList(List<AgricultureCoOperative> AgricultureCoOperative)
        {
            if (AgricultureCoOperative == null)
                throw new ArgumentNullException("AgricultureCoOperative");
            await _AgricultureCoOperativeRepository.InsertManyAsync(AgricultureCoOperative);

        }

        public async Task UpdateAgricultureCoOperative(AgricultureCoOperative AgricultureCoOperative)
        {
            if (AgricultureCoOperative == null)
                throw new ArgumentNullException("AgricultureCoOperative");
            await _AgricultureCoOperativeRepository.UpdateAsync(AgricultureCoOperative);

            //event notification
            await _mediator.EntityUpdated(AgricultureCoOperative);
        }
        public async Task UpdateAgricultureCoOperativeList(List<AgricultureCoOperative> AgricultureCoOperative)
        {
            if (AgricultureCoOperative == null)
                throw new ArgumentNullException("AgricultureCoOperative");
            await _AgricultureCoOperativeRepository.UpdateAsync(AgricultureCoOperative);

            //event notification
            //await _mediator.EntityUpdated(AgricultureCoOperative);
        }
        


    }
}
