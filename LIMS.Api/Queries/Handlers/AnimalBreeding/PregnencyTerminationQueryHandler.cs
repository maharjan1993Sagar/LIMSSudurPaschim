using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.AnimalBreeding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.AnimalBreeding
{
    public class PregnencyTerminationQueryHandler: IRequestHandler<GetQueryModels<PregnencyTerminationListDto>, IList<PregnencyTerminationListDto>>
    {
        private readonly IPregnencyTerminationService _pregnencyTerminationService;

        public PregnencyTerminationQueryHandler(IPregnencyTerminationService pregnencyTerminationService)
        {
            _pregnencyTerminationService = pregnencyTerminationService;
        }

        public async Task<IList<PregnencyTerminationListDto>> Handle(GetQueryModels<PregnencyTerminationListDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AnimalId))
            {
                var growth = await _pregnencyTerminationService.GetPregnencyTermination();
                var result = new List<PregnencyTerminationListDto>();
                foreach (var item in growth)
                {
                    result.Add(new PregnencyTerminationListDto {
                        Species = (item.AnimalRegistration.Species != null) ? item.AnimalRegistration.Species.EnglishName : "",
                        Breed = (item.AnimalRegistration.Breed != null) ? item.AnimalRegistration.Breed.EnglishName : "",
                        TerminationDate=item.TerminationDate,
                        TerminationType=item.TerminationType,
                        TerminitedBy=item.TerminitedBy,
                        Reason=item.Reason


                    });
                }
                return result;
            }
            else
            {
                var growth = await _pregnencyTerminationService.GetPregnencyTerminationByAnimalId(request.AnimalId);
                var result = new List<PregnencyTerminationListDto>();
                foreach (var item in growth)
                {
                    result.Add(new PregnencyTerminationListDto {
                        Species = (item.AnimalRegistration.Species != null) ? item.AnimalRegistration.Species.EnglishName : "",
                        Breed = (item.AnimalRegistration.Breed != null) ? item.AnimalRegistration.Breed.EnglishName : "",
                        TerminationDate = item.TerminationDate,
                        TerminationType = item.TerminationType,
                        TerminitedBy = item.TerminitedBy,
                        Reason = item.Reason


                    });
                }
                return result;
            }
        }

    }
}
