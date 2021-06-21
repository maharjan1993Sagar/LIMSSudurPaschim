using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IVetClinicService
    {
        Task<VetClinic> GetVetClinicById(string id);

        Task<IPagedList<VetClinic>> GetVetClinic(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<VetClinic>> GetVetClinic(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteVetClinic(VetClinic vetClinic);

        Task InsertVetClinic(VetClinic vetClinic);
        Task InsertVetClinicList(List<VetClinic> vetClinic);

        Task UpdateVetClinic(VetClinic vetClinic);
    }
}
