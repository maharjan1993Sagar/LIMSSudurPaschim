using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.MedicineInventory;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MedicineInventory
{
     public class MedicineDemandService:IMedicineDemandService
    {
         private readonly IRepository<MedicineDemand> _demandMedicineRepository;
       private readonly IMediator _mediator;
        public MedicineDemandService(IRepository<MedicineDemand> demandMedicineRepository, IMediator mediator)
        {
            _demandMedicineRepository = demandMedicineRepository;
            _mediator = mediator;
        }
        public async Task DeleteMedicineDemand(MedicineDemand demandMedicine)
        {
            if (demandMedicine == null)
                throw new ArgumentNullException("MedicineDemand");

            await _demandMedicineRepository.DeleteAsync(demandMedicine);

            //event notification
            await _mediator.EntityDeleted(demandMedicine);
        }

        public async Task<IPagedList<MedicineDemand>> GetMedicineDemand(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _demandMedicineRepository.Table;


            return await PagedList<MedicineDemand>.Create(query, pageIndex, pageSize);
        }

        public Task<MedicineDemand> GetMedicineDemandById(string Id)
        {
            return _demandMedicineRepository.GetByIdAsync(Id);

        }

        public async Task InsertMedicineDemand(MedicineDemand demandMedicine)
        {
            if (demandMedicine == null)
                throw new ArgumentNullException("MedicineDemand");

            await _demandMedicineRepository.InsertAsync(demandMedicine);

            //event notification
            await _mediator.EntityInserted(demandMedicine);
        }

        public async Task UpdateMedicineDemand(MedicineDemand demandMedicine)
        {
            if (demandMedicine == null)
                throw new ArgumentNullException("MedicineDemand");

            await _demandMedicineRepository.UpdateAsync(demandMedicine);

            //event notification
            await _mediator.EntityUpdated(demandMedicine);
        }

    }
}
