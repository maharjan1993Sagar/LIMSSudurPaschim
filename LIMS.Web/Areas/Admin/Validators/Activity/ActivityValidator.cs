using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Activities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Activity
{
    public class ActivityValidator:BaseLIMSValidator<ActivityModel>
    {
        public ActivityValidator(IEnumerable<IValidatorConsumer<ActivityModel>> validators,
           ILocalizationService localizationService):base(validators)
        {
            RuleFor(m => m.ActivityName).
                NotEmpty().WithMessage(localizationService.GetResource("Admin.ActivityName.Required"));
            RuleFor(m => m.FiscalYearId).NotEmpty().WithMessage(localizationService.GetResource("Admin.FiscalYear.Required"));
        }
    }
}
