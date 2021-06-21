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
    public class DiaryIndustryService:IDIaryIndustryService
    {
        private readonly IRepository<DiaryIndustryAndShop> _diaryIndustryAndShopRepository;
        private readonly IMediator _mediator;
        public DiaryIndustryService(IRepository<DiaryIndustryAndShop> diaryIndustryAndShopRepository, IMediator mediator)
        {
            _diaryIndustryAndShopRepository = diaryIndustryAndShopRepository;
            _mediator = mediator;
        }
        public async Task DeleteDiaryIndustryAndShop(DiaryIndustryAndShop DiaryIndustryAndShop)
        {
            if (DiaryIndustryAndShop == null)
                throw new ArgumentNullException("DiaryIndustryAndShop");
            await _diaryIndustryAndShopRepository.DeleteAsync(DiaryIndustryAndShop);

            //event notification
            await _mediator.EntityDeleted(DiaryIndustryAndShop);
        }

        public async Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShop(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _diaryIndustryAndShopRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            return await PagedList<DiaryIndustryAndShop>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShopByType(string createdby, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _diaryIndustryAndShopRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.OtherOrganization.Type == type);
            return await PagedList<DiaryIndustryAndShop>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShopByFiscalyear(string createdby, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _diaryIndustryAndShopRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.OtherOrganization.Type == type);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<DiaryIndustryAndShop>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<DiaryIndustryAndShop>> GetDiaryIndustryAndShopByFiscalyear(List<string> createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _diaryIndustryAndShopRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) && m.OtherOrganization.Type == type);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);
            }
            return await PagedList<DiaryIndustryAndShop>.Create(query, pageIndex, pageSize);
        }
        public Task<DiaryIndustryAndShop> GetDiaryIndustryAndShopById(string id)
        {
            return _diaryIndustryAndShopRepository.GetByIdAsync(id);
        }

        public async Task InsertDiaryIndustryAndShop(DiaryIndustryAndShop DiaryIndustryAndShop)
        {
            if (DiaryIndustryAndShop == null)
                throw new ArgumentNullException("DiaryIndustryAndShop");
            await _diaryIndustryAndShopRepository.InsertAsync(DiaryIndustryAndShop);

            //event notification
            await _mediator.EntityInserted(DiaryIndustryAndShop);
        }
        public async Task InsertDiaryIndustryAndShopList(List<DiaryIndustryAndShop> DiaryIndustryAndShop)
        {
            if (DiaryIndustryAndShop == null)
                throw new ArgumentNullException("DiaryIndustryAndShop");
            await _diaryIndustryAndShopRepository.InsertManyAsync(DiaryIndustryAndShop);

        }

        public async Task UpdateDiaryIndustryAndShop(DiaryIndustryAndShop DiaryIndustryAndShop)
        {
            if (DiaryIndustryAndShop == null)
                throw new ArgumentNullException("DiaryIndustryAndShop");
            await _diaryIndustryAndShopRepository.UpdateAsync(DiaryIndustryAndShop);

            //event notification
            await _mediator.EntityUpdated(DiaryIndustryAndShop);
        }
    }
}
