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
    public class SubSectorService:ISubSectorService
    {
        private readonly IRepository<SubSector> _SubSectorRepository;
        private readonly IMediator _mediator;
        public SubSectorService(IRepository<SubSector> SubSectorRepository, IMediator mediator)
        {
            _SubSectorRepository = SubSectorRepository;
            _mediator = mediator;
        }
        public async Task DeleteSubSector(SubSector SubSector)
        {
            if (SubSector == null)
                throw new ArgumentNullException("SubSector");

            await _SubSectorRepository.DeleteAsync(SubSector);

            //event notification
            await _mediator.EntityDeleted(SubSector);
        }
        public async Task<IPagedList<SubSector>> GetSubSector(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _SubSectorRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<SubSector>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<SubSector>> GetSubSector( int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _SubSectorRepository.Table;
            //query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<SubSector>.Create(query, pageIndex, pageSize);
        }

        //public async Task<IPagedList<SubSector>> GetSubSector(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        //{
        //    var query = _SubSectorRepository.Table;
        //    query = query.Where(m => m.CreatedBy == createdby);
        //    if (!string.IsNullOrEmpty(district))
        //    {
        //        query = query.Where(
        //          m => m.District == district
        //        );
        //    }
        //    if (!string.IsNullOrEmpty(locallevel))
        //    {
        //        query = query.Where(
        //          m => m.LocalLevel == locallevel
        //        );
        //    }

        //    return await PagedList<SubSector>.Create(query, pageIndex, pageSize);
        //}

        public async Task<IPagedList<SubSector>> GetSubSector(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _SubSectorRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<SubSector>.Create(query, pageIndex, pageSize);
        }

        public Task<SubSector> GetSubSectorById(string id)
        {
            return _SubSectorRepository.GetByIdAsync(id);
        }

        public async Task InsertSubSector(SubSector SubSector)
        {
            if (SubSector == null)
                throw new ArgumentNullException("Livestock");

            await _SubSectorRepository.InsertAsync(SubSector);

            //event notification
            await _mediator.EntityInserted(SubSector);
        }

        public Task InsertSubSectorList(List<SubSector> SubSectors)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSubSector(SubSector SubSector)
        {
            if (SubSector == null)
                throw new ArgumentNullException("baliregister");

            await _SubSectorRepository.UpdateAsync(SubSector);

            //event notification
            await _mediator.EntityUpdated(SubSector);
        }

        public Task UpdateSubSectorList(List<SubSector> SubSectors)
        {
            throw new NotImplementedException();
        }
    }
}
