using LIMS.Api.DTOs.FeedBack;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.Ainr
{
    public  class AddFeedBackCommand : IRequest<FeedbackDto>
    {
        public FeedbackDto FeedBack { get; set; }
    }
}
