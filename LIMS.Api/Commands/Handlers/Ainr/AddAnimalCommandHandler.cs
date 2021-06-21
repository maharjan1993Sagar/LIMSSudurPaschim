using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.Breed;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Ainr
{
    public class AddAnimalCommandHandler: IRequestHandler<AddAnimalCommand, AnimalRegistrationDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;

        public AddAnimalCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IFarmService farmService,
            IWorkContext workContext
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _breedService = breedService;
            _farmService = farmService;
            _speciesService = speciesService;
            _workContext = workContext;
        }

        public async Task<AnimalRegistrationDto> Handle(AddAnimalCommand request, CancellationToken cancellationToken)
        {
            var animalRegistration = request.Model.ToEntity();
            animalRegistration.Farm = await _farmService.GetFarmById(animalRegistration.FarmId);
            animalRegistration.Species = await _speciesService.GetSpeciesById(animalRegistration.SpeciesId);
            animalRegistration.Breed = await _breedService.GetBreedById(animalRegistration.BreedId);
            animalRegistration.Source= _workContext.CurrentCustomer.OrgName +"From mobile";
            animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
            await _animalRegistrationService.InsertAnimalRegistration(animalRegistration);

            //activity log

            return animalRegistration.ToModel();
        }
    }
}
