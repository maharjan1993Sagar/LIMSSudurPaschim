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
    public class GetGalleryQueryHandler: IRequestHandler<GetQueryCMS<GalleryDto>, IList<GalleryDto>>
    {
        private readonly IGalleryService _galleryService;
        private readonly IPictureService _pictureService;
        public GetGalleryQueryHandler(IGalleryService galleryService,
                                     IPictureService pictureService)
        {
            _galleryService = galleryService;
            _pictureService = pictureService;
        }
        public async Task<IList<GalleryDto>> Handle(GetQueryCMS<GalleryDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var gallery =await _galleryService.GetAll();

                var galleryDto = gallery.Where(m => m.UserId == request.UserId).Select(m=>m.ToModel()).ToList();

                //var lstGallery = new List<GalleryDto>();
                //foreach (var item in galleryDto)
                //{
                //    var objGallery = item.ToModel();
                //    objGallery.Images = item.Images.Select(m => m.ToModel()).ToList();
                //    lstGallery.Add(objGallery);
                //}
                                        

                return galleryDto;

            }
            else
            {
                return new List<GalleryDto>();
            }
        }
    }
}

