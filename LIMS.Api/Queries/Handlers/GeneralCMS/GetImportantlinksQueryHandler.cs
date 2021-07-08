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

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class GetImportantLinksQueryHandler: IRequestHandler<GetQueryCMS<ImportantLinksDto>, IList<ImportantLinksDto>>
    {
        private readonly IImportantLinksService _linksService;
        private readonly IPictureService _pictureService;
        public GetImportantLinksQueryHandler(IImportantLinksService linksService,
                                     IPictureService pictureService)
        {
            _linksService = linksService;
            _pictureService = pictureService;
        }
        public async Task<IList<ImportantLinksDto>> Handle(GetQueryCMS<ImportantLinksDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var links =await _linksService.GetAll();

                var linksDto = links.Where(m => m.UserId == request.UserId).Select(m => m.ToModel()
                ).ToList();

                return linksDto;

            }
            else
            {
                return new List<ImportantLinksDto>();
            }
        }
    }
}

