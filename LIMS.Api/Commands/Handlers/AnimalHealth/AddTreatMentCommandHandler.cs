using LIMS.Api.Commands.Models.AnimalHealth;
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
   public class AddTreatMentCommandHandler: IRequestHandler<AddTreatMentModel, TreatMentDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;
        private readonly ITreatmentService _vaccunationService;
        private readonly IDiseaseService _diseaseService;
        private readonly IFiscalYearService _fiscalYearService;

        public AddTreatMentCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            IWorkContext workContext,
            ITreatmentService vaccunationService,
            IDiseaseService diseaseService,
            IFiscalYearService fiscalYearService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _farmService = farmService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _diseaseService = diseaseService;
            _vaccunationService = vaccunationService;
        }

        public async Task<TreatMentDto> Handle(AddTreatMentModel request, CancellationToken cancellationToken)
        {
            var vaccination = request.Model.ToEntity();
            vaccination.Farm = await _farmService.GetFarmById(vaccination.FarmId);
            vaccination.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(vaccination.AnimalRegistrationId);
            vaccination.Disease = await _diseaseService.GetDiseaseById(vaccination.DiseaseName);
            vaccination.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
            vaccination.CreatedBy = _workContext.CurrentCustomer.Id;
            await _vaccunationService.InsertTreatment(vaccination);

            //activity log

            return vaccination.ToModel();
        }


    }
}
