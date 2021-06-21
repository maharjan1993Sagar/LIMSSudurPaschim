using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
    public interface IFarmForVaccinationService
    {
        Task<FarmForPurnaKhop> GetVaccinationById(string Id);
        Task<IList<FarmForPurnaKhop>> GetFarmByFiscalYear(List<string> createdby,string Fiscalyear);
        Task<IList<FarmForPurnaKhop>> GetSpeciesbyFarmName(string fiscalyear,string FarmName);

        Task<IPagedList<FarmForPurnaKhop>> GetVaccination(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FarmForPurnaKhop>> GetVaccinationByFarmId(string createdby,string FarmId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteVaccination(FarmForPurnaKhop vaccination);

        Task InsertVaccination(FarmForPurnaKhop vacination);
        Task InsertVaccinationList(List<FarmForPurnaKhop> vacination);

        Task UpdateVaccination(FarmForPurnaKhop vacination);
        Task<IPagedList<FarmForPurnaKhop>> GetVaccinationByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
