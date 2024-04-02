using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IAgricultureCoOperativeService
    {
        Task<AgricultureCoOperative> GetAgricultureCoOperativeById(string id);

        Task<IPagedList<AgricultureCoOperative>> GetAgricultureCoOperative(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AgricultureCoOperative>> GetAgricultureCoOperative(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteAgricultureCoOperative(AgricultureCoOperative AgricultureCoOperative);

        Task InsertAgricultureCoOperative(AgricultureCoOperative AgricultureCoOperative);
        Task InsertAgricultureCoOperativeList(List<AgricultureCoOperative> AgricultureCoOperative);
        Task UpdateAgricultureCoOperativeList(List<AgricultureCoOperative> AgricultureCoOperative);

        Task UpdateAgricultureCoOperative(AgricultureCoOperative AgricultureCoOperative);
    }
}
