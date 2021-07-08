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
    public class GetContactUsQueryHandler : IRequestHandler<GetQueryCMS<ContactUsDto>, IList<ContactUsDto>>
    {
        private readonly IContactUsService _contactUsService;
        public GetContactUsQueryHandler(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }
        public async Task<IList<ContactUsDto>> Handle(GetQueryCMS<ContactUsDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var all =await _contactUsService.GetAll();
                var contactByUser = all.Where(m => m.UserId == request.UserId).Select(m=>m.ToModel()).ToList();
               // var contactDto = contactByUser.ToModel();
                return contactByUser;

            }
            else
            {
                return new List<ContactUsDto>();
            }
        }
    }
}

