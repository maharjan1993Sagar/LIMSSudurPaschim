using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ISeedDistributionService
    {
        Task<SeedDistribution> GetSeedDistributionById(string id);
        Task<IPagedList<SeedDistribution>> GetSeedDistribution(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<SeedDistribution>> GetSeedDistribution(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<SeedDistribution>> GetSeedDistribution(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteSeedDistribution(SeedDistribution SeedDistribution);

        Task InsertSeedDistribution(SeedDistribution SeedDistribution);
        Task InsertSeedDistributionList(List<SeedDistribution> livestocks);

        Task UpdateSeedDistribution(SeedDistribution SeedDistribution);
        Task UpdateSeedDistributionList(List<SeedDistribution> livestocks);
    }
}
