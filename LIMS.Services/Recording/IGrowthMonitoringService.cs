using LIMS.Domain;
using LIMS.Domain.Recording;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Recording
{
   public interface IGrowthMonitoringService
    {
        Task<GrowthMonitoring> GetGrowthMonitoringById(string id);

        Task<IPagedList<GrowthMonitoring>> GetGrowthMonitoring(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteGrowthMonitoring(GrowthMonitoring growthMonitoring);

        Task InsertGrowthMonitoring(GrowthMonitoring growthMonitoring);

        Task UpdateGrowthMonitoring(GrowthMonitoring growthMonitoring);
    }
}
