using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.AnimalHealth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.AnimalHealth
{
    public class TreatmentQueryHandler: IRequestHandler<GetQueryModels<TreatMentDto>, IList<TreatMentDto>>
    {
        private readonly ITreatmentService _pregnencyDiagnosisService;

        public TreatmentQueryHandler(ITreatmentService pregnencyDiagnosisService)
        {
            _pregnencyDiagnosisService = pregnencyDiagnosisService;
        }

        public async Task<IList<TreatMentDto>> Handle(GetQueryModels<TreatMentDto> request, CancellationToken cancellationToken)
        {
            var vaccines = await _pregnencyDiagnosisService.GetTreatment();
            var result = new List<TreatMentDto>();
            foreach (var item in vaccines)
            {
                result.Add(item.ToModel());
            }
            return result;
        }

    }
}
