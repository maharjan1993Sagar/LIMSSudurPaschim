using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Services;
namespace LIMS.Services.AnimalHealth
{
   public interface IVaccinationService
    {
        Task<AnimalVaccination> GetVaccinationById(string Id);

        Task<IPagedList<AnimalVaccination>> GetVaccination(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AnimalVaccination>> GetVaccinationByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteVaccination(AnimalVaccination vaccination);

        Task InsertVaccination(AnimalVaccination vacination);

        Task UpdateVaccination(AnimalVaccination vacination);
        Task<IPagedList<AnimalVaccination>> GetVaccinationByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        int GetVaccinationCountByCustomerIds(List<string> customerid);
    }
}
