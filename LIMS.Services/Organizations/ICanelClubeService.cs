using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface ICanelClubeService
    {
        Task<CanelClube> GetCanelClubeById(string id);

        Task<IPagedList<CanelClube>> GetCanelClube(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<CanelClube>> GetCanelClube(string createdby,string keyword, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<CanelClube>> GetCanelClubeByUserList(List<string> createdby, string Lss, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteCanelClube(CanelClube canelClube);

        Task InsertCanelClube(CanelClube canelClube);
        Task InsertCanelClubeList(List<CanelClube> canelClubeAndShop);

        Task UpdateCanelClube(CanelClube canelClube);
    }
}
