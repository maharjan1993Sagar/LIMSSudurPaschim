using LIMS.Api.DTOs.AINR;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Core;
using LIMS.Domain.Data;
using LIMS.Services.Ainr;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Ainr
{
    public class GetAnimalQueryHandler : IRequestHandler<GetQueryModels<AnimalListDto>, IList<AnimalListDto>>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IWorkContext _workContext;
        public GetAnimalQueryHandler(IAnimalRegistrationService animalRegistrationService, IWorkContext workContext)
        {
            _animalRegistrationService = animalRegistrationService;
            _workContext = workContext;
        }
        public async Task<IList<AnimalListDto>> Handle(GetQueryModels<AnimalListDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.FarmId))
            {
                var animals = await _animalRegistrationService.GetAnimalRegistrationByFarmId(request.FarmId);
                var result = new List<AnimalListDto>();
                foreach (var item in animals)
                {
                    var animal =new AnimalListDto {
                        SpeciesName=(item.Species!=null)?item.Species.EnglishName:"",
                        BreedName= (item.Breed != null)?item.Breed.EnglishName:"",
                        Name=item.Name,
                        EarTagNo=item.EarTagNo,
                        Gender=item.Gender,
                        Age=item.Age,
                        AnimalColor=item.AnimalColor,
                        SireId=item.SireId,
                        DamId=item.DamId,
                        EntryType=item.EntryType,
                        DOB=item.DOB,
                        PregencyStatus=item.PregencyStatus,
                        MilkStatus=item.MilkStatus,
                        Weight=item.Weight,
                        Id=item.Id,
                        FarmName= (item.Farm != null) ? item.Farm.NameEnglish:""
                    };
                    result.Add(animal);
                }
                return result;



            }
            else if (!string.IsNullOrEmpty(request.AnimalId)){
                var animals = await _animalRegistrationService.GetAnimalRegistrationById(request.AnimalId);
                return new List<AnimalListDto> { new AnimalListDto {
                        SpeciesName=(animals.Species!=null)?animals.Species.EnglishName:"",
                        BreedName= (animals.Breed != null)?animals.Breed.EnglishName:"",
                        Name=animals.Name,
                        EarTagNo=animals.EarTagNo,
                        Gender=animals.Gender,
                        Age=animals.Age,
                        AnimalColor=animals.AnimalColor,
                        SireId=animals.SireId,
                        DamId=animals.DamId,
                        EntryType=animals.EntryType,
                        DOB=animals.DOB,
                        PregencyStatus=animals.PregencyStatus,
                        MilkStatus=animals.MilkStatus,
                        Weight=animals.Weight,
                        Id=animals.Id,
                        FarmName= (animals.Farm != null) ? animals.Farm.NameEnglish:""

                    }
            };

            }
            else
            {
                var animals = await _animalRegistrationService.GetAnimalRegistration();
                var result = new List<AnimalListDto>();
                foreach (var item in animals)
                {
                    var animal = new AnimalListDto {
                        SpeciesName = (item.Species != null) ? item.Species.EnglishName : "",
                        BreedName = (item.Breed != null) ? item.Breed.EnglishName : "",
                        Name = item.Name,
                        EarTagNo = item.EarTagNo,
                        Gender = item.Gender,
                        Age = item.Age,
                        AnimalColor = item.AnimalColor,
                        SireId = item.SireId,
                        DamId = item.DamId,
                        EntryType = item.EntryType,
                        DOB = item.DOB,
                        PregencyStatus = item.PregencyStatus,
                        MilkStatus = item.MilkStatus,
                        Weight = item.Weight,
                        Id = item.Id,
                        FarmName = (item.Farm != null) ? item.Farm.NameEnglish : ""

                    };
                    result.Add(animal);
                }
                return result;
            }
          
        }

    }
}
