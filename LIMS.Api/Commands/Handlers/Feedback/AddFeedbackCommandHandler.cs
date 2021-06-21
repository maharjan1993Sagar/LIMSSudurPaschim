using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.FeedBack;
using LIMS.Api.Extensions;
using LIMS.Services.FeedBack;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Feedback
{
    public class AddFeedbackCommandHandler : IRequestHandler<AddFeedBackCommand, FeedbackDto>
    {
        private readonly IFeedbackService _feedbackService;

        public AddFeedbackCommandHandler(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<FeedbackDto> Handle(AddFeedBackCommand request, CancellationToken cancellationToken)
        {
            var farm = request.FeedBack.ToEntity();
            await _feedbackService.InsertFeedback(farm);
            return farm.ToModel();
        }

    }
}
