using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Bali;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class GetSoilQueryHandler : IRequestHandler<GetQueryCMS<SoilDto>, IList<SoilDto>>
    {
        private readonly ISoilService _SoilService;
        private readonly IUnitService _unitService;

        public GetSoilQueryHandler(ISoilService SoilService, IUnitService unitService)
        {
            _SoilService = SoilService;
            _unitService = unitService;
        }
        public async Task<IList<SoilDto>> Handle(GetQueryCMS<SoilDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var mainMenus = await _SoilService.Getsoil(request.UserId);
                mainMenus.ToList();
                List<SoilDto> r = new List<SoilDto>();
                foreach (var item in mainMenus.ToList().OrderByDescending(m=>m.CreatedAt))
                {
                    SoilDto re = new SoilDto();
                    re = item.ToModel();
                    r.Add(re);
                }
                return r;
            }
            else
            {
                return new List<SoilDto>();
            }
        }
    }
}
