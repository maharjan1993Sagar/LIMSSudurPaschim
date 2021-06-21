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
    public class MedicineProgressService:IMedicineProgressService
    {
        private readonly IRepository<MedicineProgress> _medicineProgressRepository;
        private readonly IMediator _mediator;
        public MedicineProgressService(IRepository<MedicineProgress> medicineProgressRepository, IMediator mediator)
        {
            _medicineProgressRepository = medicineProgressRepository;
            _mediator = mediator;
        }
        public async Task DeleteMedicineProgress(MedicineProgress medicineProgress)
        {
            if (medicineProgress == null)
                throw new ArgumentNullException("MedicineProgress");

            await _medicineProgressRepository.DeleteAsync(medicineProgress);

            //event notification
            await _mediator.EntityDeleted(medicineProgress);
        }

        public async Task<IPagedList<MedicineProgress>> GetMedicineProgress(string createdBy,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _medicineProgressRepository.Table;
           
                query = query.Where(m => m.CreatedBy == createdBy);
            

            return await PagedList<MedicineProgress>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<MedicineProgress>> GetMedicineProgress(string createdBy, string month, string
            fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _medicineProgressRepository.Table;
           
              query = query.Where(
                  m => m.CreatedBy == createdBy &&
                  m.FiscalYear.Id==fiscalyear&&
                  m.Month==month
 
              );
            return await PagedList<MedicineProgress>.Create(query, pageIndex, pageSize);

        }
        public Task<MedicineProgress> GetMedicineProgressById(string Id)
        {
            return _medicineProgressRepository.GetByIdAsync(Id);

        }

        public async Task InsertMedicineProgress(MedicineProgress medicineProgress)
        {
            if (medicineProgress == null)
                throw new ArgumentNullException("MedicineProgress");

            await _medicineProgressRepository.InsertAsync(medicineProgress);

            //event notification
            await _mediator.EntityInserted(medicineProgress);
        }
        public async Task InsertMedicineProgressList(List<MedicineProgress> progressMedicines)
        {
            if (progressMedicines == null)
                throw new ArgumentNullException("progressMedicines");

            await _medicineProgressRepository.InsertManyAsync(progressMedicines);

            //event notification
            
        }

        public async Task UpdateMedicineProgress(MedicineProgress medicineProgress)
        {
            if (medicineProgress == null)
                throw new ArgumentNullException("MedicineProgress");

            await _medicineProgressRepository.UpdateAsync(medicineProgress);

            //event notification
            await _mediator.EntityUpdated(medicineProgress);
        }
        public async Task UpdateMedicineProgressList(List<MedicineProgress> progressMedicine)
        {
         
            foreach (var item in progressMedicine)
            {
                await UpdateMedicineProgress(item);
            }
        }
    }
}
