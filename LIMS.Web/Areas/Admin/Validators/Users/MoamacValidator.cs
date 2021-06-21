using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Users
{
    public class ParaProfessionValidator : BaseLIMSValidator<MoAMACModel>
    {
        public ParaProfessionValidator(
           IEnumerable<IValidatorConsumer<MoAMACModel>> validators,
           ILocalizationService localizationService)
           : base(validators)
        {
            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Moamac.EnglishName.Required"));
            RuleFor(x => x.NameNepali).NotEmpty().WithMessage(localizationService.GetResource("Admin.Moamac.NepaliName.Required"));

        }
    }
}
