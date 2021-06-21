using LIMS.Api.Commands.Models.AnimalBreeding;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalBreeding;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.AnimalBreeding
{
    public class AddPregnencyTerminationCommandHandler
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IWorkContext _workContext;
        private readonly IPregnencyTerminationService _aiService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IFiscalYearService _fiscalYearService;

        public AddPregnencyTerminationCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            IPregnencyTerminationService aiService,
            ISpeciesService speciesService,
            IBreedService breedService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _aiService = aiService;
            _speciesService = speciesService;
            _breedService = breedService;
        }


        public async Task<PregnencyTerminationDto> Handle(AddPregnencyTerminationCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            var aiService = model.ToEntity();

            aiService.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalId);
            aiService.CreatedBy = _workContext.CurrentCustomer.Id;
            aiService.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
            aiService.EntityId = _workContext.CurrentCustomer.EntityId;
            aiService.Source = _workContext.CurrentCustomer.OrgName + " From mobile";
            await _aiService.InsertPregnencyTermination(aiService);
            //activity log

            return aiService.ToModel();
        }
    }
}
