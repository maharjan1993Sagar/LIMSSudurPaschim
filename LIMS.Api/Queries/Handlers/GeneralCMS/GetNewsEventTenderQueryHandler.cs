using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Media;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LIMS.Api.Extensions;
using LIMS.Services.NewsEvent;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class GetNewsEventQueryHandler : IRequestHandler<GetQueryCMS<NewsEventTenderDto>, IList<NewsEventTenderDto>>
    {
        private readonly INewsEventService _newsEventService;
        private readonly IPictureService _pictureService;
        public GetNewsEventQueryHandler(INewsEventService newsEventService,
                                     IPictureService pictureService)
        {
            _newsEventService = newsEventService;
            _pictureService = pictureService;
        }
        public async Task<IList<NewsEventTenderDto>> Handle(GetQueryCMS<NewsEventTenderDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var newsEventTenders = await _newsEventService.GetAll();

                var newsEventDto = newsEventTenders.
                                        Where(m => m.UserId == request.UserId).
                                        Select(m => m.ToModel()).ToList();

                return newsEventDto;

            }
            else
            {
                return new List<NewsEventTenderDto>();
            }
        }
    }
}

