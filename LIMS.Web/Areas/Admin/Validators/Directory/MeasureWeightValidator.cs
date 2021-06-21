using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Directory;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Directory
{
    public class MeasureWeightValidator : BaseLIMSValidator<MeasureWeightModel>
    {
        public MeasureWeightValidator(
            IEnumerable<IValidatorConsumer<MeasureWeightModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Measures.Weights.Fields.Name.Required"));
            RuleFor(x => x.SystemKeyword).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Measures.Weights.Fields.SystemKeyword.Required"));
        }
    }
}