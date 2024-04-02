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
    public class CropDiseasesService:ICropDiseasesService
    {
        private readonly IRepository<CropDiseases> _CropDiseasesRepository;
        private readonly IMediator _mediator;
        public CropDiseasesService(IRepository<CropDiseases> CropDiseasesRepository, IMediator mediator)
        {
            _CropDiseasesRepository = CropDiseasesRepository;
            _mediator = mediator;
        }
        public async Task DeleteCropDiseases(CropDiseases CropDiseases)
        {
            if (CropDiseases == null)
                throw new ArgumentNullException("CropDiseases");

            await _CropDiseasesRepository.DeleteAsync(CropDiseases);

            //event notification
            await _mediator.EntityDeleted(CropDiseases);
        }
        public async Task<IPagedList<CropDiseases>> GetCropDiseases(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _CropDiseasesRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<CropDiseases>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<CropDiseases>> GetCropDiseases(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _CropDiseasesRepository.Table;
            if(!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalYear);
            }
            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(
                  m => m.District == district
                );
            }
            if (!string.IsNullOrEmpty(locallevel))
            {
                query = query.Where(
                  m => m.LocalLevel == locallevel
                );
            }

            return await PagedList<CropDiseases>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<CropDiseases>> GetCropDiseases(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _CropDiseasesRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<CropDiseases>.Create(query, pageIndex, pageSize);
        }

        public Task<CropDiseases> GetCropDiseasesById(string id)
        {
            return _CropDiseasesRepository.GetByIdAsync(id);
        }

        public async Task InsertCropDiseases(CropDiseases CropDiseases)
        {
            if (CropDiseases == null)
                throw new ArgumentNullException("Livestock");

            await _CropDiseasesRepository.InsertAsync(CropDiseases);

            //event notification
            await _mediator.EntityInserted(CropDiseases);
        }

        public Task InsertCropDiseasesList(List<CropDiseases> CropDiseasess)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCropDiseases(CropDiseases CropDiseases)
        {
            if (CropDiseases == null)
                throw new ArgumentNullException("baliregister");

            await _CropDiseasesRepository.UpdateAsync(CropDiseases);

            //event notification
            await _mediator.EntityUpdated(CropDiseases);
        }

        public Task UpdateCropDiseasesList(List<CropDiseases> CropDiseasess)
        {
            throw new NotImplementedException();
        }
    }
}
