using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.VaccinationInventory;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.VaccinationInventory
{
    public class ReceivedVaccinationService : IReceivedVaccinationService
    {
        private readonly IRepository<ReceivedVaccine> _receivedVaccineRepository;
        private readonly IMediator _mediator;

        public ReceivedVaccinationService(IRepository<ReceivedVaccine> receivedVaccineRepository, IMediator mediator)
        {
            _receivedVaccineRepository=receivedVaccineRepository;
            _mediator = mediator;

         
        }
        public async Task DeleteReceivedVaccine(ReceivedVaccine receivedVaccine)
        {

            if (receivedVaccine == null)
                throw new ArgumentNullException("ReceivedMedicine");

            await _receivedVaccineRepository.DeleteAsync(receivedVaccine);

            //event notification
            await _mediator.EntityDeleted(receivedVaccine);
        }

        public async Task<IPagedList<ReceivedVaccine>> GetReceivedVaccine(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _receivedVaccineRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            

            return await PagedList<ReceivedVaccine>.Create(query, pageIndex, pageSize);
        }

        public Task<ReceivedVaccine> GetReceivedVaccineById(string id)
        {
            return _receivedVaccineRepository.GetByIdAsync(id);
        }

        public async Task InsertReceivedVaccine(ReceivedVaccine receivedVaccine)
        {
            if (receivedVaccine == null)
                throw new ArgumentNullException("receivedVaccine");

            await _receivedVaccineRepository.InsertAsync(receivedVaccine);

            //event notification
            await _mediator.EntityInserted(receivedVaccine);
        }

        public async Task InsertReceivedVaccine(List<ReceivedVaccine> receivedVaccines)
        {
            if (receivedVaccines.Count < 1)
                throw new ArgumentNullException("Livestock");
            await _receivedVaccineRepository.InsertManyAsync(receivedVaccines);
        }

        public async Task UpdateReceivedVaccine(ReceivedVaccine receivedVaccine)
        {
            if (receivedVaccine == null)
                throw new ArgumentNullException("Production");

            await _receivedVaccineRepository.UpdateAsync(receivedVaccine);

            //event notification
            await _mediator.EntityUpdated(receivedVaccine);
        }
    }
}
