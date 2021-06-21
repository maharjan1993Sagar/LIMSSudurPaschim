using LIMS.Domain.BasicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public class FiscalYearForGraphService : IFiscalYearForGraphService
    {
        private readonly IRepository<FiscalYearForGraphSetup> _fiscalYearRepository;
        private readonly IMediator _mediator;
        public FiscalYearForGraphService(IRepository<FiscalYearForGraphSetup> FiscalYearRepository, IMediator mediator)
        {
            _fiscalYearRepository = FiscalYearRepository;
            _mediator = mediator;
        }
        public async Task<FiscalYearForGraphSetup> GetFiscalYear()
        {
            var query = _fiscalYearRepository.Table;
            var q = new FiscalYearForGraphSetup();
            if(query!=null)
            {
             q =   query.ToList().FirstOrDefault();

            }
            return q;

        }
        public async Task<FiscalYearForGraphSetup> GetFiscalYearById(string Id)
        {
            return await _fiscalYearRepository.GetByIdAsync(Id);

        }

        public async Task InsertFiscalYear(FiscalYearForGraphSetup fiscalYear)
        {
            if (fiscalYear == null)
                throw new ArgumentNullException("FiscalYear");

            await _fiscalYearRepository.InsertAsync(fiscalYear);

            //event notification
            await _mediator.EntityInserted(fiscalYear);
        }

        public async Task UpdateFiscalYear(FiscalYearForGraphSetup fiscalYear)
        {
            if (fiscalYear == null)
                throw new ArgumentNullException("FiscalYear");

            await _fiscalYearRepository.UpdateAsync(fiscalYear);

            //event notification
            await _mediator.EntityUpdated(fiscalYear);
        }
    }
}
