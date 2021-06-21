using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public class MeatProcessingIndustryService:IMeatProcessingIndustryService
    {
        private readonly IRepository<MeatProcesssingIndustries> _meatProcesssingIndustriesRepository;
        private readonly IMediator _mediator;
        public MeatProcessingIndustryService(IRepository<MeatProcesssingIndustries> meatProcesssingIndustriesRepository, IMediator mediator)
        {
            _meatProcesssingIndustriesRepository = meatProcesssingIndustriesRepository;
            _mediator = mediator;
        }
        public async Task DeleteMeatProcessingIndustry(MeatProcesssingIndustries meatProcesssingIndustries)
        {
            if (meatProcesssingIndustries == null)
                throw new ArgumentNullException("MeatProcesssingIndustries");
            await _meatProcesssingIndustriesRepository.DeleteAsync(meatProcesssingIndustries);

            //event notification
            await _mediator.EntityDeleted(meatProcesssingIndustries);
        }

        public async Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustry(string createdby,string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatProcesssingIndustriesRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if(string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<MeatProcesssingIndustries>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustry(List<string> createdby, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatProcesssingIndustriesRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<MeatProcesssingIndustries>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustryByType(string createdby, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatProcesssingIndustriesRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.OtherOrganization.Type == type);
            if(string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<MeatProcesssingIndustries>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustryByType(List<string> createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _meatProcesssingIndustriesRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy)  && m.OtherOrganization.Type == type);
            if (string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<MeatProcesssingIndustries>.Create(query, pageIndex, pageSize);
        }
        public Task<MeatProcesssingIndustries> GetMeatProcessingIndustryById(string id)
        {
            return _meatProcesssingIndustriesRepository.GetByIdAsync(id);
        }

        public async Task InsertMeatProcessingIndustry(MeatProcesssingIndustries meatProcesssingIndustries)
        {
            if (meatProcesssingIndustries == null)
                throw new ArgumentNullException("MeatProcesssingIndustries");
            await _meatProcesssingIndustriesRepository.InsertAsync(meatProcesssingIndustries);

            //event notification
            await _mediator.EntityInserted(meatProcesssingIndustries);
        }
        public async Task InsertMeatProcessingIndustryList(List<MeatProcesssingIndustries> MeatProcesssingIndustries)
        {
            if (MeatProcesssingIndustries == null)
                throw new ArgumentNullException("MeatProcesssingIndustries");
            await _meatProcesssingIndustriesRepository.InsertManyAsync(MeatProcesssingIndustries);

        }

        public async Task UpdateMeatProcessingIndustry(MeatProcesssingIndustries meatProcesssingIndustries)
        {
            if (meatProcesssingIndustries == null)
                throw new ArgumentNullException("MeatProcesssingIndustries");
            await _meatProcesssingIndustriesRepository.UpdateAsync(meatProcesssingIndustries);

            //event notification
            await _mediator.EntityUpdated(meatProcesssingIndustries);
        }

      
    }
}
