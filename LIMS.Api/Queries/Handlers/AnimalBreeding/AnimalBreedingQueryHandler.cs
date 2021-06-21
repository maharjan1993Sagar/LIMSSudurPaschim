using LIMS.Api.DTOs;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.AnimalBreeding;
using LIMS.Services.Recording;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.AnimalBreeding
{
    public class AnimalBreedingQueryHandler: IRequestHandler<GetQueryModels<AiListDto>, IList<AiListDto>>
    {
        private readonly IAiService _aiService;

        public AnimalBreedingQueryHandler(IAiService aiService)
        {
            _aiService = aiService;
        }

        public async Task<IList<AiListDto>> Handle(GetQueryModels<AiListDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AnimalId))
            {
                var growth = await _aiService.GetAI();
                var result = new List<AiListDto>();
                foreach (var item in growth)
                {
                    result.Add(new AiListDto{ 
                    species= (item.AnimalRegistration.Species!=null)?item.AnimalRegistration.Species.EnglishName:"",
                    Breed= (item.AnimalRegistration.Breed != null)?item.AnimalRegistration.Breed.EnglishName:"",
                  AIDate=item.AIDate,
                  RepeatAi=item.TypeOfAi,
                  BullId=item.BullId,


                    });
                }
                return result;
            }
            else
            {
                var growth = await _aiService.GetAIByAnimalId(request.AnimalId);
                var result = new List<AiListDto>();
                foreach (var item in growth)
                {
                    result.Add(new AiListDto {
                        species = (item.AnimalRegistration.Species != null) ? item.AnimalRegistration.Species.EnglishName : "",
                        Breed = (item.AnimalRegistration.Breed != null) ? item.AnimalRegistration.Breed.EnglishName : "",
                        AIDate = item.AIDate,
                        NoOfWastedSemenDose = item.NoOfWastedSemenDose,
                        RepeatAi = item.TypeOfAi,
                        BullId = item.BullId,


                    });
                }
                return result;
            }
        }

    }
}
