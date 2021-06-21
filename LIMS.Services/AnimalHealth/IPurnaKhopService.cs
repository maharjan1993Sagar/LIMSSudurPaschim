using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
    public interface IPurnaKhopService
    {
        Task<PurnaKhop> GetVaccinationById(string id);

        Task<IPagedList<PurnaKhop>> GetVaccination(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);
       
        Task DeleteVaccination(PurnaKhop vaccination);
        Task InsertVaccination(PurnaKhop vacination);
        Task InsertVaccinationList(List<PurnaKhop> vacination);
        Task UpdateVaccination(PurnaKhop vacination);
        Task<IPagedList<PurnaKhop>> GetVaccinationByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
