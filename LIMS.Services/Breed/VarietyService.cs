using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public class VarietyService : IVarietyService
    {

        private readonly IRepository<CropsProduction> _VarietyRepository;
        private readonly IMediator _mediator;
        public VarietyService(IRepository<CropsProduction> VarietyRepository, IMediator mediator)
        {
            _VarietyRepository = VarietyRepository;
            _mediator = mediator;
        }
        public async Task DeleteBreed(CropsProduction CropsProduction)
        {
            if (CropsProduction == null)
                throw new ArgumentNullException("CropsProduction");
            await _VarietyRepository.DeleteAsync(CropsProduction);

            //event notification
            await _mediator.EntityDeleted(CropsProduction);
        }

        public async Task<IPagedList<CropsProduction>> GetBreed(string createdby, string keyword, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _VarietyRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(
                    m => m.GrowingSeason.GrowingSeason.Contains(keyword)
                    ||
                    m.CropName.EnglishName.Contains(keyword)
                    ||
                     m.District.Contains(keyword)
                     ||
                     m.LocalLevel.Contains(keyword)
                     );
            }
            return await PagedList<CropsProduction>.Create(query, pageIndex, pageSize);
        }

        public Task<CropsProduction> GetBreedById(string Id)
        {
            return _VarietyRepository.GetByIdAsync(Id);
        }

        public async Task<List<CropsProduction>> GetBreedBySpeciesId(string speciesId)
        {
            var filter = Builders<CropsProduction>.Filter.Eq(x => x.CropName.Id, speciesId);
            return _VarietyRepository.Collection.Find(filter).ToList();
        }

        public async Task<IPagedList<CropsProduction>> GetFilteredProduction(string createdby, string speciesId, string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _VarietyRepository.Table;
            if (createdby != "molmac")
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (fiscalYearId != null && (speciesId == null && string.IsNullOrEmpty(district)))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId);

            }
            if (fiscalYearId != null && speciesId != null && string.IsNullOrEmpty(district))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.CropName.Species.Id == speciesId);

            }
            if (fiscalYearId != null && speciesId != null && !string.IsNullOrEmpty(district) && string.IsNullOrEmpty(LocalLevel))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.CropName.Species.Id == speciesId && m.District == district);

            }
            if (fiscalYearId != null && speciesId == null && !string.IsNullOrEmpty(district) && string.IsNullOrEmpty(LocalLevel))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.District == district);

            }
            if (fiscalYearId != null && !string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(LocalLevel))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYearId && m.LocalLevel == LocalLevel);

            }
            return await PagedList<CropsProduction>.Create(query, pageIndex, pageSize);

        }

        public async Task InsertBreed(CropsProduction CropsProduction)
        {
            if (CropsProduction == null)
                throw new ArgumentNullException("CropsProduction");
            await _VarietyRepository.InsertAsync(CropsProduction);

            //event notification
            await _mediator.EntityInserted(CropsProduction);
        }

        public async Task InsertBreed(List<CropsProduction> CropsProduction)
        {
            if (CropsProduction == null)
                throw new ArgumentNullException("CropsProduction");
            await _VarietyRepository.InsertManyAsync(CropsProduction);

        }

        public async Task UpdateBreed(CropsProduction CropsProduction)
        {
            if (CropsProduction == null)
                throw new ArgumentNullException("CropsProduction");
            await _VarietyRepository.UpdateAsync(CropsProduction);

            //event notification
            await _mediator.EntityUpdated(CropsProduction);
        }

        public async Task UpdateBreed(List<CropsProduction> Varietys)
        {
            if (Varietys == null)
                throw new ArgumentNullException("CropsProduction");

            await _VarietyRepository.UpdateAsync(Varietys);
        }

        public async Task<PagedList<CropsProduction>> GetFilteredLivestock(string createdby, string district, string locallevel, string fiscalyear, int ward, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _VarietyRepository.Table;
            if (!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(district))
            {               
                query = query.Where(m => m.LocalLevel == locallevel);
            }
            if (!String.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            if (ward>0)
            {
                query = query.Where(m => m.Ward == ward.ToString());
            }

            return await PagedList<CropsProduction>.Create(query, pageIndex, pageSize);

        }
    }
}
