﻿using LIMS.Api.Commands.Models.AnimalBreeding;
using LIMS.Api.DTOs;
using LIMS.Core;
using LIMS.Domain.AInR;
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
    public class UpdateAiCommandHandler
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;
        private readonly IAiService _aiService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IFiscalYearService _fiscalYearService;

        public UpdateAiCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            IAiService aiService,
            ISpeciesService speciesService,
            IBreedService breedService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _farmService = farmService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _aiService = aiService;
            _speciesService = speciesService;
            _breedService = breedService;
        }

        //public async Task<AIDto> Handle(UpdateAiCommand request, CancellationToken cancellationToken)
        //{
        //    //var model = request.Model;

        //    //if (string.IsNullOrEmpty(model.FarmId))
        //    //{
        //    //    var farm = new Farm() {
        //    //        NameEnglish = model.FarmName,
        //    //        MobileNo = model.MobileNo,
        //    //        CreatedBy = _workContext.CurrentCustomer.Id,
        //    //        Source = _workContext.CurrentCustomer.OrgName
        //    //    };
        //    //    await _farmService.InsertFarm(farm);
        //    //    model.FarmId = farm.Id;
        //    //}
        //    //if (string.IsNullOrEmpty(model.AnimalId))
        //    //{
        //    //    var animal = new AnimalRegistration() {
        //    //        Name = model.AnimalName,
        //    //        SpeciesId = model.SpeciesId,
        //    //        Species = await _speciesService.GetSpeciesById(model.SpeciesId),
        //    //        Breed = await _breedService.GetBreedById(model.BreedId),
        //    //        BreedId = model.BreedId,
        //    //        EarTagNo = model.Eartag,
        //    //        FarmId = model.FarmId,
        //    //        Farm = await _farmService.GetFarmById(model.FarmId),
        //    //        CreatedBy = _workContext.CurrentCustomer.Id,
        //    //        Source = _workContext.CurrentCustomer.OrgName
        //    //    };
        //    //    await _animalRegistrationService.InsertAnimalRegistration(animal);

        //    //    model.AnimalId = animal.Id;

        //    //}

        //    //var aiService = model.ToEntity();
        //    //aiService.Farm = await _farmService.GetFarmById(model.FarmId);
        //    //aiService.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalId);
        //    //aiService.CreatedBy = _workContext.CurrentCustomer.Id;
        //    //aiService.EntityId = _workContext.CurrentCustomer.EntityId;
        //    //aiService.Source = _workContext.CurrentCustomer.OrgName;
        //    //aiService.AnimalRegistrationId = model.AnimalId;
        //    //await _aiService.InsertAI(aiService);
        //    ////activity log

        //    //return aiService.ToModel();
        //}

    }
}
