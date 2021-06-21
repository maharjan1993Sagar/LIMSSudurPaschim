using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Services;
using LIMS.Services.AnimalBreeding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.AnimalHealth
{
    public class PregnencyQueryHandler : IRequestHandler<GetQueryModels<PregnencyDiagnosisDto>, IList<PregnencyDiagnosisDto>>
    {
        private readonly IPregnencyDiagnosisService _pregnencyDiagnosisService;

        public PregnencyQueryHandler(IPregnencyDiagnosisService pregnencyDiagnosisService)
        {
            _pregnencyDiagnosisService = pregnencyDiagnosisService;
        }

        public async Task<IList<PregnencyDiagnosisDto>> Handle(GetQueryModels<PregnencyDiagnosisDto> request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.AnimalId))
            {
                var vaccines = await _pregnencyDiagnosisService.GetPregnencyDiagnosis();
                var result = new List<PregnencyDiagnosisDto>();
                foreach (var item in vaccines)
                {
                    result.Add(item.ToModel());
                }
                return result;
            }
            else{
                var vaccines = await _pregnencyDiagnosisService.GetPregnencyDiagnosisByAnimalId(request.AnimalId);
                var result = new List<PregnencyDiagnosisDto>();
                foreach (var item in vaccines)
                {
                    result.Add(item.ToModel());
                }
                return result;
            }
        }

    }
}
