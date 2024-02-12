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
    public class NGOService:INGOService
    {
        private readonly IRepository<NGO> _nGORepository;
        private readonly IMediator _mediator;
        public NGOService(IRepository<NGO> nGORepository, IMediator mediator)
        {
            _nGORepository = nGORepository;
            _mediator = mediator;
        }
        public async Task DeleteNGO(NGO NGO)
        {
            if (NGO == null)
                throw new ArgumentNullException("NGO");
            await _nGORepository.DeleteAsync(NGO);

            //event notification
            await _mediator.EntityDeleted(NGO);
        }

        public async Task<IPagedList<NGO>> GetNGO(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _nGORepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id ==fiscalyear );

            }
            return await PagedList<NGO>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<NGO>> GetNGO(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _nGORepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<NGO>.Create(query, pageIndex, pageSize);
        }

        public Task<NGO> GetNGOById(string id)
        {
            return _nGORepository.GetByIdAsync(id);
        }

        public async Task InsertNGO(NGO NGO)
        {
            if (NGO == null)
                throw new ArgumentNullException("NGO");
            await _nGORepository.InsertAsync(NGO);

            //event notification
            await _mediator.EntityInserted(NGO);
        }
        public async Task InsertNGOList(List<NGO> NGO)
        {
            if (NGO == null)
                throw new ArgumentNullException("NGO");
            await _nGORepository.InsertManyAsync(NGO);

        }

        public async Task UpdateNGO(NGO NGO)
        {
            if (NGO == null)
                throw new ArgumentNullException("NGO");
            await _nGORepository.UpdateAsync(NGO);

            //event notification
            await _mediator.EntityUpdated(NGO);
        }
    }
}
