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
    public class FarmerService:IFarmerService
    {
        private readonly IRepository<Farmer> _farmerRepository;
        private readonly IMediator _mediator;
        public FarmerService(IRepository<Farmer> farmerRepository, IMediator mediator)
        {
            _farmerRepository = farmerRepository;
            _mediator = mediator;
        }
        public async Task Deletefarmer(Farmer farmer)
        {
            if (farmer == null)
                throw new ArgumentNullException("Farmer");

            await _farmerRepository.DeleteAsync(farmer);

            //event notification
            await _mediator.EntityDeleted(farmer);
        }

        public async Task<IPagedList<Farmer>> Getfarmer(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _farmerRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<Farmer>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Farmer>> GetfarmerByIncuvationCenter(string createdby, string keyword="",string fiscalyear="", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _farmerRepository.Table;
            if (!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => createdby == m.CreatedBy);
            }
            if(!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Incubation.Id == keyword);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYearId== fiscalyear);
            }
            return await PagedList<Farmer>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<Farmer>> GetfarmerByIncuvationCenter(string createdby, string district, string talimname, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _farmerRepository.Table;
            if (!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(district))
            { 
                query = query.Where(m => m.District == district);

            }
            if (!String.IsNullOrEmpty(talimname))
            {
                query = query.Where(m => m.Talim.Id == talimname);

            }
            if (!String.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
          //  query = query.Where(m => createdby == m.CreatedBy&&m.District==district&&m.Talim.Id==talimname&&m.FiscalYear.Id==fiscalyear);
           
            return await PagedList<Farmer>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<Farmer>> GetfarmerByPugigatType(string createdby, string localLevel, string budgetId, string fiscalyear, string talimId, string xetra="", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _farmerRepository.Table;
            if (!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalyear); // createdby == m.CreatedBy &&
            }
            if (!string.IsNullOrEmpty(budgetId))
            {
                query = query.Where(m => m.BudgetId == budgetId);

            }
            if (!string.IsNullOrEmpty(localLevel))
            {
                query = query.Where(m => m.LocalLevel == localLevel);

            }
            if (!string.IsNullOrEmpty(talimId))
            {
                query = query.Where(m => m.TalimId == talimId);

            }

            if (!string.IsNullOrEmpty(xetra))
            {
                query = query.Where(m => m.Budget.Xetra == xetra);

            }

            return await PagedList<Farmer>.Create(query, pageIndex, pageSize);

        }
        

        public async Task<IPagedList<Farmer>> Getfarmer(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _farmerRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<Farmer>.Create(query, pageIndex, pageSize);
        }

        public Task<Farmer> GetfarmerById(string id)
        {
            return _farmerRepository.GetByIdAsync(id);
        }

        public async Task Insertfarmer(Farmer farmer)
        {
            if (farmer == null)
                throw new ArgumentNullException("Livestock");

            await _farmerRepository.InsertAsync(farmer);

            //event notification
            await _mediator.EntityInserted(farmer);
        }

        public Task InsertfarmerList(List<Farmer> farmers)
        {
            throw new NotImplementedException();
        }

        public async Task Updatefarmer(Farmer farmer)
        {
            if (farmer == null)
                throw new ArgumentNullException("baliregister");

            await _farmerRepository.UpdateAsync(farmer);

            //event notification
            await _mediator.EntityUpdated(farmer);
        }

        public Task UpdatefarmerList(List<Farmer> farmers)
        {
            throw new NotImplementedException();
        }
    }
}
