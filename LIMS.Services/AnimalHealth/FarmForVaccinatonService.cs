using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
    public class FarmForVaccinatonService : IFarmForVaccinationService
    {
        private readonly IRepository<FarmForPurnaKhop> _animalVaccinationRepository;
        private readonly IMediator _mediator;
        public FarmForVaccinatonService(IRepository<FarmForPurnaKhop> animalVaccinationRepository, IMediator mediator)
        {
            _animalVaccinationRepository = animalVaccinationRepository;
            _mediator = mediator;
        }
        public async Task DeleteVaccination(FarmForPurnaKhop vaccination)
        {
            if (vaccination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.DeleteAsync(vaccination);
        }

        public async Task<IList<FarmForPurnaKhop>> GetFarmByFiscalYear(List<string> createdby,string Fiscalyear)
        {
            var query = _animalVaccinationRepository.Table.Where(m=>createdby.Contains(m.CreatedBy));
            query =  query.Where(m=>m.FiscalYearId == Fiscalyear);
            return   query.ToList();

        }

        public async Task<IList<FarmForPurnaKhop>> GetSpeciesbyFarmName(string fiscalyear,string FarmName)
        {
            var query = _animalVaccinationRepository.Table;
            query = query.Where(m => m.FiscalYearId == fiscalyear && m.FarmId==FarmName);
            return query.ToList();
        }

        public async Task<IPagedList<FarmForPurnaKhop>> GetVaccination(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            return await PagedList<FarmForPurnaKhop>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FarmForPurnaKhop>> GetVaccinationByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table.Where(t => customerid.Contains(t.CreatedBy));
            return await PagedList<FarmForPurnaKhop>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<FarmForPurnaKhop>> GetVaccinationByFarmId(string createdby, string FarmId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.FarmId==FarmId);
            return await PagedList<FarmForPurnaKhop>.Create(query, pageIndex, pageSize);
        }

        public Task<FarmForPurnaKhop> GetVaccinationById(string id)
        {
            return _animalVaccinationRepository.GetByIdAsync(id);
        }

        public async Task InsertVaccination(FarmForPurnaKhop vacination)
        {
            if (vacination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.InsertAsync(vacination);

            //event notification
            await _mediator.EntityInserted(vacination);
        }
        public async Task InsertVaccinationList(List<FarmForPurnaKhop> vacination)
        {
            if (vacination == null)
                throw new ArgumentNullException("CanelClube");
            await _animalVaccinationRepository.InsertManyAsync(vacination);

        }


        public async Task UpdateVaccination(FarmForPurnaKhop vacination)
        {
            if(vacination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.UpdateAsync(vacination);

            //event notification
            await _mediator.EntityUpdated(vacination);
        }
    }
}
