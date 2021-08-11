using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
   public  interface IFarmLabResourceService
    {
        Task<FarmLabResources> GetfarmLabResourcesById(string id);
        Task<IPagedList<FarmLabResources>> GetfarmLabResources(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<FarmLabResources>> GetfarmLabResources(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeletefarmLabResources(FarmLabResources farmLabResources);

        Task InsertfarmLabResources(FarmLabResources farmLabResources);
        Task InsertfarmLabResourcesList(List<FarmLabResources> livestocks);

        Task UpdatefarmLabResources(FarmLabResources farmLabResources);
        Task UpdatefarmLabResourcesList(List<FarmLabResources> livestocks);

    }
}
