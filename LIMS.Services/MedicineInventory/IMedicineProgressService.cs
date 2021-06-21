using LIMS.Domain;
using LIMS.Domain.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MedicineInventory
{
    public interface IMedicineProgressService
    {
        Task<MedicineProgress> GetMedicineProgressById(string Id);
        Task<IPagedList<MedicineProgress>> GetMedicineProgress(string createdBy,
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<MedicineProgress>> GetMedicineProgress(string createdBy,string month,string
            fiscalyear,int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteMedicineProgress(MedicineProgress progressMedicine);


        Task InsertMedicineProgress(MedicineProgress progressMedicine);
        Task InsertMedicineProgressList(List<MedicineProgress> progressMedicines);

        Task UpdateMedicineProgress(MedicineProgress progressMedicine);
        Task UpdateMedicineProgressList(List<MedicineProgress> progressMedicine);

    }
}
