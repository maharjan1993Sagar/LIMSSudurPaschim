﻿using LIMS.Api.Commands.Models.AnimalHealth;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.AnimalHealth
{
   public class UpdateTreatMentCommandHandler: IRequestHandler<UpdateTreatMentModel, TreatMentDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;
        private readonly ITreatmentService _vaccunationService;

        public UpdateTreatMentCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            IWorkContext workContext,
            ITreatmentService vaccunationService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _farmService = farmService;
            _workContext = workContext;
            _vaccunationService = vaccunationService;
        }


        public async Task<TreatMentDto> Handle(UpdateTreatMentModel request, CancellationToken cancellationToken)
        {
            var treatment = await _vaccunationService.GetTreatmentById(request.Model.Id);
            if (treatment != null)
            {
                var vaccination = request.Model.ToEntity(treatment);
                vaccination.Farm = await _farmService.GetFarmById(vaccination.FarmId);
                vaccination.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(vaccination.AnimalRegistrationId);
                vaccination.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
                vaccination.CreatedBy = _workContext.CurrentCustomer.Id;
                await _vaccunationService.InsertTreatment(vaccination);

                //activity log

                return vaccination.ToModel();
            }
            return null;
        }


    }
}
