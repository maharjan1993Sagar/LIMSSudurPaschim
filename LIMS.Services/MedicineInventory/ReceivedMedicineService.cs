using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.MedicineInventory;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MedicineInventory
{
    public class ReceivedMedicineService:IReceivedMedicineService
    {

        private readonly IRepository<ReceivedMedicine> _receivedMedicineRepository;
        private readonly IMediator _mediator;
        public ReceivedMedicineService(IRepository<ReceivedMedicine> receivedMedicineRepository, IMediator mediator)
        {
            _receivedMedicineRepository = receivedMedicineRepository;
            _mediator = mediator;
        }
        public async Task DeleteReceivedMedicine(ReceivedMedicine receivedMedicine)
        {
            if (receivedMedicine == null)
                throw new ArgumentNullException("ReceivedMedicine");

            await _receivedMedicineRepository.DeleteAsync(receivedMedicine);

            //event notification
            await _mediator.EntityDeleted(receivedMedicine);
        }

        public async Task<IPagedList<ReceivedMedicine>> GetReceivedMedicine(string createdBy="",int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _receivedMedicineRepository.Table;
            if (!String.IsNullOrWhiteSpace(createdBy)){
                query = query.Where(m => m.CreatedBy == createdBy);
            }

            return await PagedList<ReceivedMedicine>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ReceivedMedicine>> GetReceivedMedicine(string createdBy, string fiscalyear, string month, string organization,
         int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _receivedMedicineRepository.Table;
            
                query = query.Where(m => m.CreatedBy == createdBy
                && m.FiscalYear.Id==fiscalyear&&
                m.Month==month&&
                m.Organization.Id==organization

                );
            

            return await PagedList<ReceivedMedicine>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ReceivedMedicine>> GetReceivedMedicine(string createdBy, string fiscalyear, string month,
       int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _receivedMedicineRepository.Table;

            query = query.Where(m => m.CreatedBy == createdBy
            && m.FiscalYear.Id == fiscalyear &&
            m.Month == month 

            );


            return await PagedList<ReceivedMedicine>.Create(query, pageIndex, pageSize);
        }
        public Task<ReceivedMedicine> GetReceivedMedicineById(string Id)
        {
            return _receivedMedicineRepository.GetByIdAsync(Id);

        }

        public async Task InsertReceivedMedicine(ReceivedMedicine receivedMedicine)
        {
            if (receivedMedicine == null)
                throw new ArgumentNullException("ReceivedMedicine");

            await _receivedMedicineRepository.InsertAsync(receivedMedicine);

            //event notification
            await _mediator.EntityInserted(receivedMedicine);
        }
        public async Task InsertReceivedMedicineList(List<ReceivedMedicine> receivedMedicines)
        {
            if (receivedMedicines == null)
                throw new ArgumentNullException("ReceivedMedicine");
            await _receivedMedicineRepository.InsertManyAsync(receivedMedicines);

        }

        public async  Task UpdateReceivedMedicineList(List<ReceivedMedicine> receivedMedicines)
        {

            await _receivedMedicineRepository.UpdateAsync(receivedMedicines);

        }
        public async Task UpdateReceivedMedicine(ReceivedMedicine receivedMedicine)
        {
            if (receivedMedicine == null)
                throw new ArgumentNullException("ReceivedMedicine");

            await _receivedMedicineRepository.UpdateAsync(receivedMedicine);

            //event notification
            await _mediator.EntityUpdated(receivedMedicine);
        }
    }
}
