using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Recording;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Recording
{
    public class GrowthMonitoringService:IGrowthMonitoringService
    {

        private readonly IRepository<GrowthMonitoring> _growthMonitoringRepository;
        private readonly IMediator _mediator;
        public GrowthMonitoringService(IRepository<GrowthMonitoring> growthMonitoringRepository, IMediator mediator)
        {
            _growthMonitoringRepository = growthMonitoringRepository;
            _mediator = mediator;
        }

        public async Task DeleteGrowthMonitoring(GrowthMonitoring growthMonitoring)
        {
            if (growthMonitoring == null)
                throw new ArgumentNullException("GrowthMonitoring");
            await _growthMonitoringRepository.DeleteAsync(growthMonitoring);

            //event notification
            await _mediator.EntityDeleted(growthMonitoring);
        }

        public async Task<IPagedList<GrowthMonitoring>> GetGrowthMonitoring(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _growthMonitoringRepository.Table;
            return await PagedList<GrowthMonitoring>.Create(query, pageIndex, pageSize);
        }

        public Task<GrowthMonitoring> GetGrowthMonitoringById(string id)
        {
            return _growthMonitoringRepository.GetByIdAsync(id);
        }

        public async Task InsertGrowthMonitoring(GrowthMonitoring growthMonitoring)
        {
            if (growthMonitoring == null)
                throw new ArgumentNullException("GrowthMonitoring");
            await _growthMonitoringRepository.InsertAsync(growthMonitoring);

            //event notification
            await _mediator.EntityInserted(growthMonitoring);
        }

        public async Task UpdateGrowthMonitoring(GrowthMonitoring growthMonitoring)
        {
            if (growthMonitoring == null)
                throw new ArgumentNullException("GrowthMonitoring");
            await _growthMonitoringRepository.UpdateAsync(growthMonitoring);

            //event notification
            await _mediator.EntityUpdated(growthMonitoring);
        }

    }

}

