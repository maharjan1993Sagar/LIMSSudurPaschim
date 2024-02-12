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
    public class LabambitKrishakService:ILabambitKrishakService
    {
        private readonly IRepository<LabambitKrishakHaru> _LabambitKrishakHaruRepository;
        private readonly IMediator _mediator;
        public LabambitKrishakService(IRepository<LabambitKrishakHaru> LabambitKrishakHaruRepository, IMediator mediator)
        {
            _LabambitKrishakHaruRepository = LabambitKrishakHaruRepository;
            _mediator = mediator;
        }
        public async Task DeleteLabambitKrishakHaru(LabambitKrishakHaru LabambitKrishakHaru)
        {
            if (LabambitKrishakHaru == null)
                throw new ArgumentNullException("LabambitKrishakHaru");

            await _LabambitKrishakHaruRepository.DeleteAsync(LabambitKrishakHaru);

            //event notification
            await _mediator.EntityDeleted(LabambitKrishakHaru);
        }

        public async Task<IPagedList<LabambitKrishakHaru>> GetLabambitKrishakHaru(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _LabambitKrishakHaruRepository.Table;
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

            return await PagedList<LabambitKrishakHaru>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<LabambitKrishakHaru>> GetFilteredLabambitKrishak(string id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _LabambitKrishakHaruRepository.Table;
            query = query.Where(m => m.CreatedBy==id &&m.PujigatKharchaKharakram.Type==type&&m.PujigatKharchaKharakram.ProgramType==programType&&m.FiscalYear.Id==fiscalYear);
           

            return await PagedList<LabambitKrishakHaru>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<LabambitKrishakHaru>> GetFilteredLabambitKrishak(string id, string fiscalYear, string programType, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _LabambitKrishakHaruRepository.Table;
            query = query.Where(m => m.CreatedBy == id  && m.PujigatKharchaKharakram.Id == programType && m.FiscalYear.Id == fiscalYear);


            return await PagedList<LabambitKrishakHaru>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<LabambitKrishakHaru>> GetLabambitKrishakHaru(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _LabambitKrishakHaruRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<LabambitKrishakHaru>.Create(query, pageIndex, pageSize);
        }

        public Task<LabambitKrishakHaru> GetLabambitKrishakHaruById(string id)
        {
            return _LabambitKrishakHaruRepository.GetByIdAsync(id);
        }

        public async Task InsertLabambitKrishakHaru(LabambitKrishakHaru LabambitKrishakHaru)
        {
            if (LabambitKrishakHaru == null)
                throw new ArgumentNullException("Livestock");

            await _LabambitKrishakHaruRepository.InsertAsync(LabambitKrishakHaru);

            //event notification
            await _mediator.EntityInserted(LabambitKrishakHaru);
        }

        public Task InsertLabambitKrishakHaruList(List<LabambitKrishakHaru> LabambitKrishakHarus)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateLabambitKrishakHaru(LabambitKrishakHaru LabambitKrishakHaru)
        {
            if (LabambitKrishakHaru == null)
                throw new ArgumentNullException("baliregister");

            await _LabambitKrishakHaruRepository.UpdateAsync(LabambitKrishakHaru);

            //event notification
            await _mediator.EntityUpdated(LabambitKrishakHaru);
        }

        public Task UpdateLabambitKrishakHaruList(List<LabambitKrishakHaru> LabambitKrishakHarus)
        {
            throw new NotImplementedException();
        }
    }

}
