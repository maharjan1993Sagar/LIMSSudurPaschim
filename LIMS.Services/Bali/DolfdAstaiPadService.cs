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
    public class DolfdAstaiPadService:IDolfdAstaiPadService
    {
        private readonly IRepository<DolfdSthaiTahaEntry> _DolfdSthaiTahaEntryRepository;
        private readonly IMediator _mediator;
        public DolfdAstaiPadService(IRepository<DolfdSthaiTahaEntry> DolfdSthaiTahaEntryRepository, IMediator mediator)
        {
            _DolfdSthaiTahaEntryRepository = DolfdSthaiTahaEntryRepository;
            _mediator = mediator;
        }
        public async Task DeleteTahaData(DolfdSthaiTahaEntry DolfdSthaiTahaEntry)
        {
            if (DolfdSthaiTahaEntry == null)
                throw new ArgumentNullException("DolfdSthaiTahaEntry");

            await _DolfdSthaiTahaEntryRepository.DeleteAsync(DolfdSthaiTahaEntry);

            //event notification
            await _mediator.EntityDeleted(DolfdSthaiTahaEntry);
        }

        public async Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _DolfdSthaiTahaEntryRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DolfdSthaiTahaEntry>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(string createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _DolfdSthaiTahaEntryRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.NepaliFiscalYear.Contains(fiscalyear)

                );
            }

            return await PagedList<DolfdSthaiTahaEntry>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _DolfdSthaiTahaEntryRepository.Table;
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DolfdSthaiTahaEntry>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<DolfdSthaiTahaEntry>> GetTahaData(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _DolfdSthaiTahaEntryRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DolfdSthaiTahaEntry>.Create(query, pageIndex, pageSize);
        }

        public Task<DolfdSthaiTahaEntry> GetTahaDataById(string id)
        {
            return _DolfdSthaiTahaEntryRepository.GetByIdAsync(id);
        }

        public async Task InsertTahaData(DolfdSthaiTahaEntry DolfdSthaiTahaEntry)
        {
            if (DolfdSthaiTahaEntry == null)
                throw new ArgumentNullException("Livestock");

            await _DolfdSthaiTahaEntryRepository.InsertAsync(DolfdSthaiTahaEntry);

            //event notification
            await _mediator.EntityInserted(DolfdSthaiTahaEntry);
        }

        public Task InsertTahaDataList(List<DolfdSthaiTahaEntry> DolfdSthaiTahaEntrys)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTahaData(DolfdSthaiTahaEntry DolfdSthaiTahaEntry)
        {
            if (DolfdSthaiTahaEntry == null)
                throw new ArgumentNullException("baliregister");

            await _DolfdSthaiTahaEntryRepository.UpdateAsync(DolfdSthaiTahaEntry);

            //event notification
            await _mediator.EntityUpdated(DolfdSthaiTahaEntry);
        }

        public Task UpdateTahaDataList(List<DolfdSthaiTahaEntry> DolfdSthaiTahaEntrys)
        {
            throw new NotImplementedException();
        }
    }
}
