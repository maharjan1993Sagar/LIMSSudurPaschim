using LIMS.Api.DTOs.RationBalance;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.RationBalance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.RationBalance
{
    public class AnimalFeedQueryHandler
    {
        private readonly IAnimalFeedService _rationBalanceServcie;

        public AnimalFeedQueryHandler(IAnimalFeedService animalFeedService)
        {
            _rationBalanceServcie = animalFeedService;
        }

        public async Task<IList<AnimalFeedDto>> Handle(GetQueryModels<AnimalFeedDto> request, CancellationToken cancellationToken)
        {
            var feeds = await _rationBalanceServcie.GetFeedLibraries();
            var result = new List<AnimalFeedDto>();
            foreach (var item in feeds)
            {
                result.Add(item.ToModel());
            }
            return result;
        }
    }
}
