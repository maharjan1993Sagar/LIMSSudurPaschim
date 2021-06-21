using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Localization;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Localization
{
    public class LanguageResourceValidator : BaseLIMSValidator<LanguageResourceModel>
    {
        public LanguageResourceValidator(
            IEnumerable<IValidatorConsumer<LanguageResourceModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Name.Required"));
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Value.Required"));
        }
    }
}