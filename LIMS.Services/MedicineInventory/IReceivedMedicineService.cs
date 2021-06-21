using LIMS.Domain;
using LIMS.Domain.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MedicineInventory
{
    public interface IReceivedMedicineService
    {
        Task<ReceivedMedicine> GetReceivedMedicineById(string Id);
        Task<IPagedList<ReceivedMedicine>> GetReceivedMedicine(string createdBy="",
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ReceivedMedicine>> GetReceivedMedicine(string createdBy,string fiscalyear,string month,string organization,
         int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ReceivedMedicine>> GetReceivedMedicine(string createdBy, string fiscalyear, string month,
         int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteReceivedMedicine(ReceivedMedicine receivedMedicine);


        Task InsertReceivedMedicine(ReceivedMedicine receivedMedicine);
        Task InsertReceivedMedicineList(List<ReceivedMedicine> receivedMedicines);


        Task UpdateReceivedMedicineList(List<ReceivedMedicine> receivedMedicines);
        Task UpdateReceivedMedicine(ReceivedMedicine receivedMedicine);
    }
}
