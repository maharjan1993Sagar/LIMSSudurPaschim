using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
    public class SampleService:ISampleService
    {
        private readonly IRepository<Sample> _sampleRepository;
        private readonly IMediator _mediator;
        public SampleService(IRepository<Sample> sampleRepository, IMediator mediator)
        {
            _sampleRepository = sampleRepository;
            _mediator = mediator;
        }

        public async Task Deletesample(Sample sample)
        {
            if (sample == null)
                throw new ArgumentNullException("Sample");
            await _sampleRepository.DeleteAsync(sample);

            //event notification
            await _mediator.EntityDeleted(sample);
        }

        public async Task<IPagedList<Sample>> Getsample(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _sampleRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<Sample>.Create(query, pageIndex, pageSize);
        }


        public Task<Sample> GetsampleById(string id)
        {
            return _sampleRepository.GetByIdAsync(id);
        }
        
        public async Task Insertsample(Sample sample)
        {
            if (sample == null)
                throw new ArgumentNullException("Sample");
            await _sampleRepository.InsertAsync(sample);

            //event notification
            await _mediator.EntityInserted(sample);
        }

        public async Task Updatesample(Sample sample)
        {
            if (sample == null)
                throw new ArgumentNullException("Sample");
            await _sampleRepository.UpdateAsync(sample);

            //event notification
            await _mediator.EntityUpdated(sample);
        }

    }
}
