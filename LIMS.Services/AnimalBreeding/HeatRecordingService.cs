using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public class HeatRecordingService:IHeatRecordingService
    {
        private readonly IRepository<HeatRecording> _heatRecordingRepository;
        private readonly IMediator _mediator;
        public HeatRecordingService(IRepository<HeatRecording> heatRecordingRepository, IMediator mediator)
        {
            _heatRecordingRepository = heatRecordingRepository;
            _mediator = mediator;
        }

        public async Task DeleteHeatRecording(HeatRecording heatRecording)
        {
            if (heatRecording == null)
                throw new ArgumentNullException("HeatRecording");
            await _heatRecordingRepository.DeleteAsync(heatRecording);

            //event notification
            await _mediator.EntityDeleted(heatRecording);
        }

        public async Task<IPagedList<HeatRecording>> GetHeatRecording(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _heatRecordingRepository.Table;
            return await PagedList<HeatRecording>.Create(query, pageIndex, pageSize);
        }

      
        public Task<HeatRecording> GetHeatRecordingById(string id)
        {
            return _heatRecordingRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<HeatRecording>> GetHeatRecordingByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _heatRecordingRepository.Table;
            query = query.Where(m => m.AnimalRegistration.Id == animalId);
            return await PagedList<HeatRecording>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertHeatRecording(HeatRecording heatRecording)
        {
            if (heatRecording == null)
                throw new ArgumentNullException("HeatRecording");
            await _heatRecordingRepository.InsertAsync(heatRecording);

            //event notification
            await _mediator.EntityInserted(heatRecording);
        }

        public async Task UpdateHeatRecording(HeatRecording heatRecording)
        {
            if (heatRecording == null)
                throw new ArgumentNullException("HeatRecording");
            await _heatRecordingRepository.UpdateAsync(heatRecording);

            //event notification
            await _mediator.EntityUpdated(heatRecording);
        }

    }
}
