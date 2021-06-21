using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Breed
{
    public class SpeciesValidator:BaseLIMSValidator<SpeciesModel>
    {
        public SpeciesValidator(
 IEnumerable<IValidatorConsumer<SpeciesModel>> validators,
 ILocalizationService localizationService) : base(validators)
        {
            RuleFor(x => x.EnglishName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Species.EnglishName.Required"));
        }
    }
}
