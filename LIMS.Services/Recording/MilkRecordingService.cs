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
    public class MilkRecordingService:IMilkRecordingService
    {
        private readonly IRepository<MilkRecording> _milkRecordingRepository;
        private readonly IMediator _mediator;
        public MilkRecordingService(IRepository<MilkRecording> milkRecordingRepository, IMediator mediator)
        {
            _milkRecordingRepository = milkRecordingRepository;
            _mediator = mediator;
        }

        public async Task DeleteMilkRecording(MilkRecording milkRecording)
        {
            if (milkRecording == null)
                throw new ArgumentNullException("MilkRecording");
            await _milkRecordingRepository.DeleteAsync(milkRecording);

            //event notification
            await _mediator.EntityDeleted(milkRecording);
        }

        public async Task<IPagedList<MilkRecording>> GetMilkRecording(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _milkRecordingRepository.Table;
            return await PagedList<MilkRecording>.Create(query, pageIndex, pageSize);
        }

        public Task<MilkRecording> GetMilkRecordingById(string id)
        {
            return _milkRecordingRepository.GetByIdAsync(id);
        }

        public async Task InsertMilkRecording(MilkRecording milkRecording)
        {
            if (milkRecording == null)
                throw new ArgumentNullException("MilkRecording");
            await _milkRecordingRepository.InsertAsync(milkRecording);

            //event notification
            await _mediator.EntityInserted(milkRecording);
        }

        public async Task UpdateMilkRecording(MilkRecording milkRecording)
        {
            if (milkRecording == null)
                throw new ArgumentNullException("MilkRecording");
            await _milkRecordingRepository.UpdateAsync(milkRecording);

            //event notification
            await _mediator.EntityUpdated(milkRecording);
        }

    }

}

