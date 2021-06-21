using LIMS.Domain;
using LIMS.Domain.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MedicineInventory
{
    public interface IMedicineDemandService
    {
        Task<MedicineDemand> GetMedicineDemandById(string Id);
        Task<IPagedList<MedicineDemand>> GetMedicineDemand(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteMedicineDemand(MedicineDemand medicineDemand);


        Task InsertMedicineDemand(MedicineDemand medicineDemand);


        Task UpdateMedicineDemand(MedicineDemand medicineDemand);
    }
}
