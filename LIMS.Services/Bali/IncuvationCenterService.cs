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
   public class IncubationCenterService:IIncuvationCenterService
    {
        private readonly IRepository<IncubationCenter> _incuvationCenterRepository;
        private readonly IMediator _mediator;
        public IncubationCenterService(IRepository<IncubationCenter> incuvationCenterRepository, IMediator mediator)
        {
            _incuvationCenterRepository = incuvationCenterRepository;
            _mediator = mediator;
        }
        public async Task DeleteincuvationCenter(IncubationCenter incuvationCenter)
        {
            if (incuvationCenter == null)
                throw new ArgumentNullException("incubationCenter");

            await _incuvationCenterRepository.DeleteAsync(incuvationCenter);

            //event notification
            await _mediator.EntityDeleted(incuvationCenter);
        }

        public async Task<IPagedList<IncubationCenter>> GetincuvationCenter(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _incuvationCenterRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<IncubationCenter>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<IncubationCenter>> GetincuvationCenter(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _incuvationCenterRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<IncubationCenter>.Create(query, pageIndex, pageSize);
        }

        public Task<IncubationCenter> GetincuvationCenterById(string id)
        {
            return _incuvationCenterRepository.GetByIdAsync(id);
        }

        public async Task InsertincuvationCenter(IncubationCenter incuvationCenter)
        {
            if (incuvationCenter == null)
                throw new ArgumentNullException("Livestock");

            await _incuvationCenterRepository.InsertAsync(incuvationCenter);

            //event notification
            await _mediator.EntityInserted(incuvationCenter);
        }

        public Task InsertincuvationCenterList(List<IncubationCenter> incuvationCenters)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateincuvationCenter(IncubationCenter incuvationCenter)
        {
            if (incuvationCenter == null)
                throw new ArgumentNullException("baliregister");

            await _incuvationCenterRepository.UpdateAsync(incuvationCenter);

            //event notification
            await _mediator.EntityUpdated(incuvationCenter);
        }

        public Task UpdateincuvationCenterList(List<IncubationCenter> incuvationCenters)
        {
            throw new NotImplementedException();
        }
    }
}
