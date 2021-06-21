using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface ILivestockResearchCenterService
    {
        Task<LivestockResearchCenter> GetLivestockResearchCenterById(string id);

        Task<IPagedList<LivestockResearchCenter>> GetLivestockResearchCenter(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<LivestockResearchCenter>> GetLivestockResearchCenter(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteLivestockResearchCenter(LivestockResearchCenter livestockResearchCenter);

        Task InsertLivestockResearchCenterList(List<LivestockResearchCenter> livestockResearchCenter);
        Task InsertLivestockResearchCenter(LivestockResearchCenter livestockResearchCenter);
        Task UpdateLivestockResearchCenter(LivestockResearchCenter livestockResearchCenter);

    }
}
