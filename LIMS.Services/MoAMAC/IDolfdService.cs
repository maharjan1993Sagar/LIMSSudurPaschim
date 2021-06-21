using LIMS.Domain;
using LIMS.Domain.MoAMAC;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public interface IDolfdService
    {
        Task<Dolfd> GetDolfdById(string id);

        Task<IPagedList<Dolfd>> GetDolfd(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteDolfd(Dolfd dolfd);

        Task InsertDolfd(Dolfd dolfd);

        Task UpdateDolfd(Dolfd dolfd);

        Task<IPagedList<Dolfd>> GetDolfdByMolmacId(string molmacId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task UpdateDolfd(List<Dolfd> dolfd);
    }
}
