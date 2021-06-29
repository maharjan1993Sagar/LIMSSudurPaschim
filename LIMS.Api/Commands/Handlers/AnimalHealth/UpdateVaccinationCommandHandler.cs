using LIMS.Api.Commands.Models.AnimalHealth;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.AnimalHealth
{
    public class UpdateVaccinationCommandHandler : IRequestHandler<UpdateVaccinationCommand, VaccinationDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;
        private readonly IVaccinationService _vaccunationService;
        private readonly IFiscalYearService _fiscalYearService;

        public UpdateVaccinationCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            IWorkContext workContext
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _farmService = farmService;
            _workContext = workContext;
        }

        public async Task<VaccinationDto> Handle(UpdateVaccinationCommand request, CancellationToken cancellationToken)
        {
            var vaccination = await _vaccunationService.GetVaccinationById(request.vaccinationDto.Id);
            if (vaccination != null)
            {
                vaccination = request.vaccinationDto.ToEntity(vaccination);
                vaccination.Farm = await _farmService.GetFarmById(vaccination.FarmId);
                vaccination.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(vaccination.AnimalRegistrationId);
                vaccination.FiscalYear = await _fiscalYearService.GetFiscalYearById(vaccination.FiscalYearId);
                vaccination.FiscalYear = await _fiscalYearService.GetFiscalYearById(vaccination.FiscalYearId);

                vaccination.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
                vaccination.UpdatedBy = _workContext.CurrentCustomer.Id;
                await _vaccunationService.InsertVaccination(vaccination);

                //activity log

                return vaccination.ToModel();
            }
            return null;
        }

    }
}
