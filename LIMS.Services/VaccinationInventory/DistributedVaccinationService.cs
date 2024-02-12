using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.VaccinationInventory;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.VaccinationInventory
{
    public class DistributedVaccinationService:IDistributedVaccinationService
    {
        private readonly IRepository<DistributedVaccine> _distributedVaccineRepository;
        private readonly IMediator _mediator;


        public DistributedVaccinationService(IRepository<DistributedVaccine> distributedVaccineRepository, IMediator mediator)
        {
            _distributedVaccineRepository = distributedVaccineRepository;
            _mediator = mediator;


        }
        public async Task DeleteDistributedVaccine(DistributedVaccine distributedVaccine)
        {

            if (distributedVaccine == null)
                throw new ArgumentNullException("ReceivedMedicine");

            await _distributedVaccineRepository.DeleteAsync(distributedVaccine);

            //event notification
            await _mediator.EntityDeleted(distributedVaccine);
        }

        public async Task<IPagedList<DistributedVaccine>> GetDistributedVaccine(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _distributedVaccineRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }

            return await PagedList<DistributedVaccine>.Create(query, pageIndex, pageSize);
        }

        public Task<DistributedVaccine> GetDistributedVaccineById(string id)
        {
            return _distributedVaccineRepository.GetByIdAsync(id);
        }

        public async Task InsertDistributedVaccine(DistributedVaccine distributedVaccine)
        {
            if (distributedVaccine == null)
                throw new ArgumentNullException("distributedVaccine");

            await _distributedVaccineRepository.InsertAsync(distributedVaccine);

            //event notification
            await _mediator.EntityInserted(distributedVaccine);
        }

        public async Task InsertDistributedVaccine(List<DistributedVaccine> distributedVaccines)
        {
            if (distributedVaccines.Count < 1)
                throw new ArgumentNullException("distributedVaccine");
            await _distributedVaccineRepository.InsertManyAsync(distributedVaccines);
        }

        public async Task UpdateDistributedVaccine(DistributedVaccine distributedVaccine)
        {
            if (distributedVaccine == null)
                throw new ArgumentNullException("distributedVaccine");

            await _distributedVaccineRepository.UpdateAsync(distributedVaccine);

            //event notification
            await _mediator.EntityUpdated(distributedVaccine);
        }
    }
}
