using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Vaccination
{
    public class VaccinationTypeValidater:BaseLIMSValidator<VaccinationTypeModel>
    {
        public VaccinationTypeValidater(
           IEnumerable<IValidatorConsumer<VaccinationTypeModel>> validators,
           ILocalizationService localizationService)
           : base(validators)
        {
            RuleFor(x => x.MedicalName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vaccination.VaccinationType.Name.Required"));
        }
    }
}
