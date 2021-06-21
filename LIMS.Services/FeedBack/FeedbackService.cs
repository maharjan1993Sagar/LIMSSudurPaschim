using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Feedback;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.FeedBack
{
    public class FeedbackService : IFeedbackService
    {

        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IMediator _mediator;
        public FeedbackService(IRepository<Feedback> feedbackRepository, IMediator mediator)
        {
            _feedbackRepository = feedbackRepository;
            _mediator = mediator;
        }
        public async Task DeleteFeedback(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentNullException("Feedback");

            await _feedbackRepository.DeleteAsync(feedback);

            //event notification
            await _mediator.EntityDeleted(feedback);
        }

        public async Task<IPagedList<Feedback>> GetFeedback(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _feedbackRepository.Table;


            return await PagedList<Feedback>.Create(query, pageIndex, pageSize);
        }

        public Task<Feedback> GetFeedbackById(string Id)
        {
            return _feedbackRepository.GetByIdAsync(Id);

        }

        public async Task InsertFeedback(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentNullException("Feedback");

            await _feedbackRepository.InsertAsync(feedback);

            //event notification
            await _mediator.EntityInserted(feedback);
        }

        public async Task UpdateFeedback(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentNullException("Feedback");

            await _feedbackRepository.UpdateAsync(feedback);

            //event notification
            await _mediator.EntityUpdated(feedback);
        }

    }

}


