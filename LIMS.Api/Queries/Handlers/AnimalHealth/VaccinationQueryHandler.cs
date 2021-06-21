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
    public class VaccinationQueryHandler : IRequestHandler<GetQueryModels<VaccinationDto>, IList<VaccinationDto>>
    {
        private readonly IVaccinationService _vaccinationService;

        public VaccinationQueryHandler(IVaccinationService vaccinationType)
        {
            _vaccinationService = vaccinationType;
        }

        public async Task<IList<VaccinationDto>> Handle(GetQueryModels<VaccinationDto> request, CancellationToken cancellationToken)
        {
            var vaccines = await _vaccinationService.GetVaccination();
            var result = new List<VaccinationDto>();
            foreach (var item in vaccines)
            {
                result.Add(item.ToModel());
            }
            return result;
        }

    }
}
