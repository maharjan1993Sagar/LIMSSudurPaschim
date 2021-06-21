using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Directory;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Directory
{
    public class MeasureUnitValidator : BaseLIMSValidator<MeasureUnitModel>
    {
        public MeasureUnitValidator(
            IEnumerable<IValidatorConsumer<MeasureUnitModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Measures.Units.Fields.Name.Required"));
        }
    }
}