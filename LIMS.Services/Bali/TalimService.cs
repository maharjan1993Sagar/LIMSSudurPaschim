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
    public class TalimService:ITalimService
    {
        private readonly IRepository<Talim> _talimRepository;
        private readonly IMediator _mediator;
        public TalimService(IRepository<Talim> talimRepository, IMediator mediator)
        {
            _talimRepository = talimRepository;
            _mediator = mediator;
        }
        public async Task Deletetalim(Talim talim)
        {
            if (talim == null)
                throw new ArgumentNullException("Talim");

            await _talimRepository.DeleteAsync(talim);

            //event notification
            await _mediator.EntityDeleted(talim);
        }

        public async Task<IPagedList<Talim>> Gettalim(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _talimRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYearId == fiscalyear
                );
            }

            return await PagedList<Talim>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<Talim>> Gettalim(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _talimRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<Talim>.Create(query, pageIndex, pageSize);
        }

        public Task<Talim> GettalimById(string id)
        {
            return _talimRepository.GetByIdAsync(id);
        }

        public async Task Inserttalim(Talim talim)
        {
            if (talim == null)
                throw new ArgumentNullException("Livestock");

            await _talimRepository.InsertAsync(talim);

            //event notification
            await _mediator.EntityInserted(talim);
        }

        public Task InserttalimList(List<Talim> talims)
        {
            throw new NotImplementedException();
        }

        public async Task Updatetalim(Talim talim)
        {
            if (talim == null)
                throw new ArgumentNullException("baliregister");

            await _talimRepository.UpdateAsync(talim);

            //event notification
            await _mediator.EntityUpdated(talim);
        }

        public Task UpdatetalimList(List<Talim> talims)
        {
            throw new NotImplementedException();
        }

    }
}
