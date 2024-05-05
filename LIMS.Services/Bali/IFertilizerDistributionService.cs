using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IFertilizerDistributionService
    {
        Task<FertilizerDistribution> GetFertilizerDistributionById(string id);
        Task<IPagedList<FertilizerDistribution>> GetFertilizerDistribution(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FertilizerDistribution>> GetFertilizerDistribution(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<FertilizerDistribution>> GetFertilizerDistribution(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteFertilizerDistribution(FertilizerDistribution FertilizerDistribution);

        Task InsertFertilizerDistribution(FertilizerDistribution FertilizerDistribution);
        Task InsertFertilizerDistributionList(List<FertilizerDistribution> livestocks);

        Task UpdateFertilizerDistribution(FertilizerDistribution FertilizerDistribution);
        Task UpdateFertilizerDistributionList(List<FertilizerDistribution> livestocks);
    }
}
