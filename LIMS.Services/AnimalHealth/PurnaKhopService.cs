using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
    public class PurnaKhopService : IPurnaKhopService
    {
        private readonly IRepository<PurnaKhop> _animalVaccinationRepository;
        private readonly IMediator _mediator;
        public PurnaKhopService(IRepository<PurnaKhop> animalVaccinationRepository, IMediator mediator)
        {
            _animalVaccinationRepository = animalVaccinationRepository;
            _mediator = mediator;
        }
        public async Task DeleteVaccination(PurnaKhop vaccination)
        {
            if(vaccination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.DeleteAsync(vaccination);
        }

        public async  Task<IPagedList<PurnaKhop>> GetVaccination(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            return await PagedList<PurnaKhop>.Create(query, pageIndex, pageSize);
        }

        public async   Task<IPagedList<PurnaKhop>> GetVaccinationByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalVaccinationRepository.Table.Where(t => customerid.Contains(t.CreatedBy));
            return await PagedList<PurnaKhop>.Create(query, pageIndex, pageSize);
        }

        

        public Task<PurnaKhop> GetVaccinationById(string id)
        {
            return _animalVaccinationRepository.GetByIdAsync(id);
        }

        public async Task InsertVaccination(PurnaKhop vacination)
        {
            if (vacination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.InsertAsync(vacination);

            //event notification
            await _mediator.EntityInserted(vacination);
        }
        public async Task InsertVaccinationList(List<PurnaKhop> vacination)
        {
            if (vacination == null)
                throw new ArgumentNullException("CanelClube");
            await _animalVaccinationRepository.InsertManyAsync(vacination);

        }
        public async Task UpdateVaccination(PurnaKhop vacination)
        {
            if (vacination == null)
                throw new ArgumentNullException("Vaccination");
            await _animalVaccinationRepository.UpdateAsync(vacination);

            //event notification
            await _mediator.EntityUpdated(vacination);
        }
    }
}
