using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.FeedBack;
using LIMS.Api.Extensions;
using LIMS.Services.FeedBack;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Feedback
{
    public class UpdateFeedbackCommandHandler : IRequestHandler<AddFeedBackCommand, FeedbackDto>
    {
        private readonly IFeedbackService _feedbackService;

        public UpdateFeedbackCommandHandler(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<FeedbackDto> Handle(AddFeedBackCommand request, CancellationToken cancellationToken)
        {
            var feedback = await _feedbackService.GetFeedbackById(request.FeedBack.Id);
            if (feedback != null)
            {
                feedback = request.FeedBack.ToEntity(feedback);
                await _feedbackService.UpdateFeedback(feedback);

                //activity log

                return feedback.ToModel();
            }
            return null;
        }
    }
}
