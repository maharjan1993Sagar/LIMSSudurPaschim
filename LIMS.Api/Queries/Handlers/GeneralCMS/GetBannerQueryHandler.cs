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
    public class GetBannerQueryHandler: IRequestHandler<GetQueryCMS<BannerDto>, IList<BannerDto>>
    {
        private readonly IBannerService _bannerService;
        private readonly IPictureService _pictureService;
        public GetBannerQueryHandler(IBannerService bannerService,
                                     IPictureService pictureService)
        {
            _bannerService = bannerService;
            _pictureService = pictureService;
        }
        public async Task<IList<BannerDto>> Handle(GetQueryCMS<BannerDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var banner =await _bannerService.GetAll();

                var bannerDto = banner.Where(m => m.UserId == request.UserId&&m.IsActive==true).Select(m => new BannerDto {
                    BannerId = m.BannerId.ToString(),
                    Title = m.Title,
                    UserId = m.UserId,
                    Image = m.Image.ToModel()
                }).ToList();

                return bannerDto;

            }
            else
            {
                return new List<BannerDto>();
            }
        }
    }
}

