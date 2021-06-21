using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface INGOService
    {
        Task<NGO> GetNGOById(string id);

        Task<IPagedList<NGO>> GetNGO(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<NGO>> GetNGO(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteNGO(NGO nGo);

        Task InsertNGO(NGO nGo);
        Task InsertNGOList(List<NGO> nGo);

        Task UpdateNGO(NGO nGo);

    }
}
