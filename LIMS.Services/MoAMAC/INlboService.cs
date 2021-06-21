using LIMS.Domain;
using LIMS.Domain.MoAMAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
   public interface INlboService
    {
        Task<Nlbo> GetNlboById(string id);

        Task<IPagedList<Nlbo>> GetNlbo(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteNlbo(Nlbo nlbo);

        Task InsertNlbo(Nlbo nlbo);

        Task UpdateNlbo(Nlbo nlbo);

       

    }
}
