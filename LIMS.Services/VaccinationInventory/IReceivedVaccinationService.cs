using LIMS.Domain;
using LIMS.Domain.VaccinationInventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.VaccinationInventory
{
    public interface IReceivedVaccinationService
    {
        Task<ReceivedVaccine> GetReceivedVaccineById(string id);
        Task<IPagedList<ReceivedVaccine>> GetReceivedVaccine(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);
     
        Task DeleteReceivedVaccine(ReceivedVaccine receivedVaccine);

        Task InsertReceivedVaccine(ReceivedVaccine receivedVaccine);
        Task InsertReceivedVaccine(List<ReceivedVaccine> livestocks);

        Task UpdateReceivedVaccine(ReceivedVaccine receivedVaccine);
        
    }
}
