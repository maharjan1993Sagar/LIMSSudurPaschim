using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.Extensions;
using LIMS.Services.Ainr;
using LIMS.Services.Breed;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Ainr
{
    public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, AnimalRegistrationDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IFarmService _farmService;

        public UpdateAnimalCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IFarmService farmService)
        {
            _animalRegistrationService = animalRegistrationService;
            _breedService = breedService;
            _farmService = farmService;
            _speciesService = speciesService;
        }

        public async Task<AnimalRegistrationDto> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            var animalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(request.Model.Id);
            if (animalRegistration != null)
            {
                animalRegistration = request.Model.ToEntity(animalRegistration);
                animalRegistration.Farm = await _farmService.GetFarmById(animalRegistration.FarmId);
                animalRegistration.Species = await _speciesService.GetSpeciesById(animalRegistration.SpeciesId);
                animalRegistration.Breed = await _breedService.GetBreedById(animalRegistration.BreedId);
                await _animalRegistrationService.UpdateAnimalRegistration(animalRegistration);

                //activity log

                return animalRegistration.ToModel();
            }
            return null;
        }

    }
}
