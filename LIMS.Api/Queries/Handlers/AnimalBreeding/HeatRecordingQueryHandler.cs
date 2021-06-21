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
    public class HeatRecordingQueryHandler: IRequestHandler<GetQueryModels<HeatListDto>, IList<HeatListDto>>
    {
        private readonly IHeatRecordingService _heatRecordingService;

        public HeatRecordingQueryHandler(IHeatRecordingService heatRecordingService)
        {
            _heatRecordingService = heatRecordingService;
        }

        public async Task<IList<HeatListDto>> Handle(GetQueryModels<HeatListDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AnimalId))
            {
                var growth = await _heatRecordingService.GetHeatRecording();
                var result = new List<HeatListDto>();
                foreach (var item in growth)
                {
                    result.Add(new HeatListDto {
                        Species = (item.AnimalRegistration.Species != null) ? item.AnimalRegistration.Species.EnglishName : "",
                        Breed = (item.AnimalRegistration.Breed != null) ? item.AnimalRegistration.Breed.EnglishName : "",
                         HeatDate= item.HeatDate,
                        NoOfHeat = item.NoOfHeat,
                        Description = item.Description,


                    });
                }
                return result;
            }
            else
            {
                var growth = await _heatRecordingService.GetHeatRecordingByAnimalId(request.AnimalId);
                var result = new List<HeatListDto>();
                foreach (var item in growth)
                {
                    result.Add(new HeatListDto {
                        Species = (item.AnimalRegistration.Species != null) ? item.AnimalRegistration.Species.EnglishName : "",
                        Breed = (item.AnimalRegistration.Breed != null) ? item.AnimalRegistration.Breed.EnglishName : "",
                        HeatDate = item.HeatDate,
                        NoOfHeat = item.NoOfHeat,
                        Description = item.Description,


                    });
                }
                return result;
            }
        }

    }
}
