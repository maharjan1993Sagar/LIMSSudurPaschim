using LIMS.Api.Commands.Models;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Localization;
using LIMS.Services.Vaccination;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.AnimalHealth
{
    public class AddVaccinationCommandHandler : IRequestHandler<AddVaccinationCommand, VaccinationDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;
        private readonly IVaccinationService _vaccunationService;
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly IFiscalYearService _fiscalYearService;

        public AddVaccinationCommandHandler(
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            IWorkContext workContext,
            IVaccinationService vaccunationService,
            IFiscalYearService fiscalYearService,
            IVaccinationTypeService vaccinationTypeService
        )
        {
            _animalRegistrationService = animalRegistrationService;
            _farmService = farmService;
            _workContext = workContext;
            _vaccinationTypeService = vaccinationTypeService;
            _fiscalYearService = fiscalYearService;
            _vaccunationService = vaccunationService;
        }

        public async Task<VaccinationDto> Handle(AddVaccinationCommand request, CancellationToken cancellationToken)
        {
            var vaccination = request.VaccinationDto.ToEntity();
            vaccination.Farm = await _farmService.GetFarmById(vaccination.FarmId);
            vaccination.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(vaccination.AnimalRegistrationId);
            vaccination.FiscalYear = await _fiscalYearService.GetFiscalYearById(vaccination.FiscalYearId);
            vaccination.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
            vaccination.CreatedBy = _workContext.CurrentCustomer.Id;
            vaccination.VaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(vaccination.VaccinationTypeId);
            await _vaccunationService.InsertVaccination(vaccination);

            //activity log

            return vaccination.ToModel();
        }

    }
}
