using LIMS.Domain;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
  public  class FiscalYearService:IFiscalYearService
    {

        private readonly IRepository<FiscalYear> _fiscalYearRepository;
        private readonly IMediator _mediator;
        public FiscalYearService(IRepository<FiscalYear> FiscalYearRepository, IMediator mediator)
        {
            _fiscalYearRepository = FiscalYearRepository;
            _mediator = mediator;
        }
        public async Task DeleteFiscalYear(FiscalYear fiscalYear)
        {
            if (fiscalYear == null)
                throw new ArgumentNullException("FiscalYear");

            await _fiscalYearRepository.DeleteAsync(fiscalYear);

            //event notification
            await _mediator.EntityDeleted(fiscalYear);
        }

        public async Task<IPagedList<FiscalYear>> GetFiscalYear(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fiscalYearRepository.Table;


            return await PagedList<FiscalYear>.Create(query, pageIndex, pageSize);
        }

        public Task<FiscalYear> GetFiscalYearById(string Id)
        {
            return _fiscalYearRepository.GetByIdAsync(Id);

        }

        public async Task InsertFiscalYear(FiscalYear fiscalYear)
        {
            if (fiscalYear == null)
                throw new ArgumentNullException("FiscalYear");

            await _fiscalYearRepository.InsertAsync(fiscalYear);

            //event notification
            await _mediator.EntityInserted(fiscalYear);
        }

        public async Task UpdateFiscalYear(FiscalYear fiscalYear)
        {
            if (fiscalYear == null)
                throw new ArgumentNullException("FiscalYear");

            await _fiscalYearRepository.UpdateAsync(fiscalYear);

            //event notification
            await _mediator.EntityUpdated(fiscalYear);
        }
        public async Task UpdateFiscalYear(List<FiscalYear> fiscalYear)
        {
            if (fiscalYear == null)
                throw new ArgumentNullException("FiscalYear");

            await _fiscalYearRepository.UpdateAsync(fiscalYear);

        }
        public async Task<FiscalYear> GetCurrentFiscalYear()
        {
            var query = _fiscalYearRepository.Table;
            query = query.Where(m => m.CurrentFiscalYear == true);
             return await query.FirstOrDefaultAsync();
        }

    }
}
