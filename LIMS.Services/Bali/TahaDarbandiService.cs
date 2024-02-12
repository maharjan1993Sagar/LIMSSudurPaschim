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
    public class TahaDarbandiService: ITahaDarbandiService
    {
        private readonly IRepository<DolfdTahaEntry> _DolfdTahaEntryRepository;
        private readonly IMediator _mediator;
        public TahaDarbandiService(IRepository<DolfdTahaEntry> DolfdTahaEntryRepository, IMediator mediator)
        {
            _DolfdTahaEntryRepository = DolfdTahaEntryRepository;
            _mediator = mediator;
        }
        public async Task DeleteTahaData(DolfdTahaEntry DolfdTahaEntry)
        {
            if (DolfdTahaEntry == null)
                throw new ArgumentNullException("DolfdTahaEntry");

            await _DolfdTahaEntryRepository.DeleteAsync(DolfdTahaEntry);

            //event notification
            await _mediator.EntityDeleted(DolfdTahaEntry);
        }

        public async Task<IPagedList<DolfdTahaEntry>> GetTahaData(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _DolfdTahaEntryRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DolfdTahaEntry>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<DolfdTahaEntry>> GetTahaData(string createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _DolfdTahaEntryRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.NepaliFiscalYear.Contains(fiscalyear)
                 
                );
            }

            return await PagedList<DolfdTahaEntry>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<DolfdTahaEntry>> GetTahaData(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _DolfdTahaEntryRepository.Table;
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DolfdTahaEntry>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<DolfdTahaEntry>> GetTahaData(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _DolfdTahaEntryRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DolfdTahaEntry>.Create(query, pageIndex, pageSize);
        }

        public Task<DolfdTahaEntry> GetTahaDataById(string id)
        {
            return _DolfdTahaEntryRepository.GetByIdAsync(id);
        }

        public async Task InsertTahaData(DolfdTahaEntry DolfdTahaEntry)
        {
            if (DolfdTahaEntry == null)
                throw new ArgumentNullException("Livestock");

            await _DolfdTahaEntryRepository.InsertAsync(DolfdTahaEntry);

            //event notification
            await _mediator.EntityInserted(DolfdTahaEntry);
        }

        public Task InsertTahaDataList(List<DolfdTahaEntry> DolfdTahaEntrys)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTahaData(DolfdTahaEntry DolfdTahaEntry)
        {
            if (DolfdTahaEntry == null)
                throw new ArgumentNullException("baliregister");

            await _DolfdTahaEntryRepository.UpdateAsync(DolfdTahaEntry);

            //event notification
            await _mediator.EntityUpdated(DolfdTahaEntry);
        }

        public Task UpdateTahaDataList(List<DolfdTahaEntry> DolfdTahaEntrys)
        {
            throw new NotImplementedException();
        }
    }
}
