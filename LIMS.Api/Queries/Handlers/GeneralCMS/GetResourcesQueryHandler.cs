using LIMS.Api.DTOs;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class GetResourcesQueryHandler : IRequestHandler<GetQueryCMS<Resources>, IList<Resources>>
    {
        private readonly IFarmLabResourceService _farmLabResourceService;
        private readonly IUnitService _unitService;

        public GetResourcesQueryHandler(IFarmLabResourceService farmLabResourceService, IUnitService unitService)
        {
            _farmLabResourceService = farmLabResourceService;
            _unitService = unitService;
        }
        public async Task<IList<Resources>> Handle(GetQueryCMS<Resources> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var mainMenus = await _farmLabResourceService.GetfarmLabResources(request.UserId);
                List<Resources> r = new List<Resources>();
                foreach (var item in mainMenus)
                {
                    Resources re = new Resources();
                    re = item.ToModel();
                    r.Add(re);
                }
                r.ForEach(m => m.UnitId = _unitService.GetUnitById(m.UnitId).Result.UnitNameEnglish);
                return r;
            }
            else
            {
                return new List<Resources>();
            }
        }
    }
}
