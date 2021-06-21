using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Settings;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Settings
{
    public class SettingValidator : BaseLIMSValidator<SettingModel>
    {
        public SettingValidator(
            IEnumerable<IValidatorConsumer<SettingModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.AllSettings.Fields.Name.Required"));
        }
    }
}