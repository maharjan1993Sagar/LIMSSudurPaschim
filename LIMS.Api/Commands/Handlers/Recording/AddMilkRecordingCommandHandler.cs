using LIMS.Api.Commands.Models;
using LIMS.Api.Commands.Models.Performance;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Localization;
using LIMS.Services.Recording;
using LIMS.Services.Vaccination;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.AnimalHealth
{
    public class AddMilkRecordingCommandHandler : IRequestHandler<AddMilkCommand, MilkRecordingDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IMilkRecordingService _milkRecordingService;
     

        public AddMilkRecordingCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IWorkContext workContext,
           IMilkRecordingService milkRecordingService,
            IFiscalYearService fiscalYearService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _milkRecordingService = milkRecordingService;

        }

        public async Task<MilkRecordingDto> Handle(AddMilkCommand request, CancellationToken cancellationToken)
        {
            var milk = request.MilkDto.ToEntity();
            milk.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(request.MilkDto.AnimalRegistrationId);
            milk.CreatedBy = _workContext.CurrentCustomer.Id;
            milk.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
            await _milkRecordingService.InsertMilkRecording(milk);
            return milk.ToModel();
        }

      
    }
}
