using LIMS.Domain;
using LIMS.Domain.SemenDistribution;
using LIMS.Domain.StatisticalData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Semen
{
    public interface ISemenDistributionService
    {
        Task<SemenDistribution> GetSemenDistributionById(string id);
        Task<IPagedList<SemenDistribution>> GetSemenDistribution(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteSemenDistribution(SemenDistribution semenDistribution);

        Task InsertSemenDIstribution(SemenDistribution semenDistribution);
        Task InsertSemenDistributionList(List<SemenDistribution> semenDistributions);

        Task UpdateSemenDistribution(SemenDistribution semenDistribution);

       
    }
}
