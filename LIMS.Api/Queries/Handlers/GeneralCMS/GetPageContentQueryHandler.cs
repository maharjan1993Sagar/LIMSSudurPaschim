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
    public class GetPageContentQueryHandler: IRequestHandler<GetQueryCMS<PageContentDto>, IList<PageContentDto>>
    {
        private readonly IPageContentService _pageContentService;
        private readonly IPictureService _pictureService;
        public GetPageContentQueryHandler(IPageContentService pageContentService,
                                     IPictureService pictureService)
        {
            _pageContentService = pageContentService;
            _pictureService = pictureService;
        }
        public async Task<IList<PageContentDto>> Handle(GetQueryCMS<PageContentDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var pageContent = await _pageContentService.GetAll();

                var pageContentDto = pageContent.Where(m => m.UserId == request.UserId).Select(m => m.ToModel()).ToList();

                return pageContentDto;

            }
            else
            {
                return new List<PageContentDto>();
            }
        }
    }
}

