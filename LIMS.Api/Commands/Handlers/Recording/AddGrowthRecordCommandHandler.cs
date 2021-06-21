using LIMS.Api.Commands.Models.Performance;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Recording;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Recording
{
    public class AddGrowthRecordCommandHandler: IRequestHandler<AddGrowthCommand, GrowthMonitoringDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IGrowthMonitoringService _growthMonitoringService;


        public AddGrowthRecordCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IWorkContext workContext,
           IGrowthMonitoringService growthMonitoringService,
            IFiscalYearService fiscalYearService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _growthMonitoringService = growthMonitoringService;

        }

        public async Task<GrowthMonitoringDto> Handle(AddGrowthCommand request, CancellationToken cancellationToken)
        {
            var growth = request.Model.ToEntity();
            growth.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(request.Model.AnimalRegistrationId);
            growth.CreatedBy = _workContext.CurrentCustomer.Id;
            growth.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
            await _growthMonitoringService.InsertGrowthMonitoring(growth);
            return growth.ToModel();
        }

    }
}
