using LIMS.Domain;
using LIMS.Domain.MoAMAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public interface ILssService
    {
        Task<Lss> GetLssById(string id);

        Task<IPagedList<Lss>> GetLss(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteLss(Lss lss);

        Task InsertLss(Lss lss);
        Task UpdateLss(Lss lss);

        Task<IPagedList<Lss>> GetLssByVhlsecId(string vhlsecId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task UpdateLss(List<Lss> lss);
    }
}
