using LIMS.Domain;
using LIMS.Domain.MoAMAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public interface IVhlsecService
    {
        Task<Vhlsec> GetVhlsecById(string id);

        Task<IPagedList<Vhlsec>> GetVhlsec(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteVhlsec(Vhlsec vhlsec);

        Task InsertVhlsec(Vhlsec vhlsec);

        Task UpdateVhlsec(Vhlsec vhlsec);

        Task<IPagedList<Vhlsec>> GetVhlsecByDolfdId(string dolfdId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task UpdateVhlsec(List<Vhlsec> vhlsec);
    }
}
