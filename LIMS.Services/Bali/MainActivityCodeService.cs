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
    public class MainActivityCodeService:IMainActivityCodeService
    {
        private readonly IRepository<MainActivityCode> _MainActivityCodeRepository;
        private readonly IMediator _mediator;
        public MainActivityCodeService(IRepository<MainActivityCode> MainActivityCodeRepository, IMediator mediator)
        {
            _MainActivityCodeRepository = MainActivityCodeRepository;
            _mediator = mediator;
        }
        public async Task DeleteMainActivityCode(MainActivityCode MainActivityCode)
        {
            if (MainActivityCode == null)
                throw new ArgumentNullException("MainActivityCode");

            await _MainActivityCodeRepository.DeleteAsync(MainActivityCode);

            //event notification
            await _mediator.EntityDeleted(MainActivityCode);
        }
        public async Task<IPagedList<MainActivityCode>> GetMainActivityCode(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MainActivityCodeRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<MainActivityCode>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<MainActivityCode>> GetMainActivityCode(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MainActivityCodeRepository.Table;
           // query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<MainActivityCode>.Create(query, pageIndex, pageSize);
        }

        //public async Task<IPagedList<MainActivityCode>> GetMainActivityCode(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        //{
        //    var query = _MainActivityCodeRepository.Table;
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

        //    return await PagedList<MainActivityCode>.Create(query, pageIndex, pageSize);
        //}

        public async Task<IPagedList<MainActivityCode>> GetMainActivityCode(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _MainActivityCodeRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<MainActivityCode>.Create(query, pageIndex, pageSize);
        }

        public Task<MainActivityCode> GetMainActivityCodeById(string id)
        {
            return _MainActivityCodeRepository.GetByIdAsync(id);
        }

        public async Task InsertMainActivityCode(MainActivityCode MainActivityCode)
        {
            if (MainActivityCode == null)
                throw new ArgumentNullException("Livestock");

            await _MainActivityCodeRepository.InsertAsync(MainActivityCode);

            //event notification
            await _mediator.EntityInserted(MainActivityCode);
        }

        public Task InsertMainActivityCodeList(List<MainActivityCode> MainActivityCodes)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMainActivityCode(MainActivityCode MainActivityCode)
        {
            if (MainActivityCode == null)
                throw new ArgumentNullException("baliregister");

            await _MainActivityCodeRepository.UpdateAsync(MainActivityCode);

            //event notification
            await _mediator.EntityUpdated(MainActivityCode);
        }

        public Task UpdateMainActivityCodeList(List<MainActivityCode> MainActivityCodes)
        {
            throw new NotImplementedException();
        }

        public async Task<Boolean> IsExistsCode(string code)
        {
            var query = _MainActivityCodeRepository.Table;
            var isexists = await query.AnyAsync(m => m.Limbis_Code == code.Trim());

            return isexists;
           
        }
    }
}
