using LIMS.Domain;
using LIMS.Domain.VaccinationInventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.VaccinationInventory
{
    public  interface IDistributedVaccinationService
    {
        Task<DistributedVaccine> GetDistributedVaccineById(string id);
        Task<IPagedList<DistributedVaccine>> GetDistributedVaccine(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteDistributedVaccine(DistributedVaccine distributedVaccine);

        Task InsertDistributedVaccine(DistributedVaccine distributedVaccine);
        Task InsertDistributedVaccine(List<DistributedVaccine> distributedVaccine);

        Task UpdateDistributedVaccine(DistributedVaccine distributedVaccine);

    }
}
