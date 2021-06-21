using LIMS.Api.Commands.Models.AnimalHealth;
using LIMS.Api.Commands.Models.Performance;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Recording;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.AnimalHealth
{
    public class UpdateMilkRecordCOmmandHandler : IRequestHandler<UpdateMilkCOmmand, MilkRecordingDto>
    {

        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IMilkRecordingService _milkRecordingService;


        public UpdateMilkRecordCOmmandHandler(
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

        public async Task<MilkRecordingDto> Handle(UpdateMilkCOmmand request, CancellationToken cancellationToken)
        {
            var milk = await _milkRecordingService.GetMilkRecordingById(request.MilkDto.Id);
            if (milk != null)
            {
                milk = request.MilkDto.ToEntity(milk);
                milk.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(milk.AnimalRegistrationId);
                milk.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
                milk.UpdatedBy = _workContext.CurrentCustomer.Id;
                await _milkRecordingService.InsertMilkRecording(milk);
                return milk.ToModel();
   
            }
            return null;
        }

    }
}
