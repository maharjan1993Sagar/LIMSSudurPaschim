using LIMS.Domain;
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
    public class VaccinationService:IVaccinationService
    {
        private readonly IRepository<AnimalVaccination> _animalVaccinationRepository;
        private readonly IMediator _mediator;
        public VaccinationService(IRepository<AnimalVaccination> animalVaccinationRepository, IMediator mediator)
        {
            _animalVaccinationRepository = animalVaccinationRepository;
            _mediator = mediator;
        }

        public async Task DeleteVaccination(AnimalVaccination animalVaccination)
        {
            if (animalVaccination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.DeleteAsync(animalVaccination);

            //event notification
           await _mediator.EntityDeleted(animalVaccination);
        }

        public async Task<IPagedList<AnimalVaccination>> GetVaccination(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table;
            return  await PagedList<AnimalVaccination>.Create(query, pageIndex, pageSize);
        }


        public Task<AnimalVaccination> GetVaccinationById(string id)
        {
            return _animalVaccinationRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<AnimalVaccination>> GetVaccinationByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table;
            query = query.Where(m => m.AnimalRegistration.Id == animalId);
            return await PagedList<AnimalVaccination>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertVaccination(AnimalVaccination animalVaccination)
        {
            if (animalVaccination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.InsertAsync(animalVaccination);

            //event notification
            await _mediator.EntityInserted(animalVaccination);
        }

        public async Task UpdateVaccination(AnimalVaccination animalVaccination)
        {
            if (animalVaccination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.UpdateAsync(animalVaccination);

            //event notification
            await _mediator.EntityUpdated(animalVaccination);
        }

        public async Task<IPagedList<AnimalVaccination>> GetVaccinationByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table.Where(t => customerid.Contains(t.CreatedBy));


            return await PagedList<AnimalVaccination>.Create(query, pageIndex, pageSize);
        }
        public int GetVaccinationCountByCustomerIds(List<string> customerid)
        {
            var query = _animalVaccinationRepository.Table.Where(t => customerid.Contains(t.CreatedBy));


            return query.Count();
        }
    }
}
