using LIMS.Api.DTOs;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.RationBalance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.RationBalance
{
   public  class FeedLibraryQueryHandler: IRequestHandler<GetQueryModels<RationBalanceDto>, IList<RationBalanceDto>>
    {
            private readonly IRationBalanceService _rationBalanceServcie;

            public FeedLibraryQueryHandler(IRationBalanceService rationBalanceService)
            {
            _rationBalanceServcie = rationBalanceService;
            }

            public async Task<IList<RationBalanceDto>> Handle(GetQueryModels<RationBalanceDto> request, CancellationToken cancellationToken)
            {
                var feeds = await _rationBalanceServcie.GetFeedLibraries();
                var result = new List<RationBalanceDto>();
                foreach (var item in feeds)
                {
                    result.Add(item.ToModel());
                }
                return result;
            }

        }
    }

