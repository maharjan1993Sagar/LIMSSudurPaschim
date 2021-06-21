using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Customers;
using LIMS.Services.Vaccination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.AnimalHealth
{
    public class VaccineQueryHandler : IRequestHandler<GetVaccinationQuery, IList<VaccineDto>>
    {
        private readonly IVaccinationTypeService _vaccinationService;

        public VaccineQueryHandler(IVaccinationTypeService vaccinationType)
        {
            _vaccinationService = vaccinationType;
        }

        public async Task<IList<VaccineDto>> Handle(GetVaccinationQuery request, CancellationToken cancellationToken)
        {
            var vaccines = await _vaccinationService.FiletrVaccinationType("Vaccine");
            var result = new List<VaccineDto>();
            foreach (var item in vaccines)
            {
                result.Add(item.ToModel());
            }
            return result;
        }

    }
}
