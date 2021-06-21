using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Molmac
{
    public class NlboValidator : BaseLIMSValidator<NlboModel>
    {
        public NlboValidator(IEnumerable<IValidatorConsumer<NlboModel>> validators,
         ILocalizationService localizationService) : base(validators)
        {

            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.EnglishName.Required"));
            RuleFor(x => x.Provience).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.Province.Required"));
            RuleFor(x => x.Address).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.Address.Required"));
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.Email.Required"));
            RuleFor(x => x.UserNameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.UserName.Required"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.Password.Required"));
        }
    }
}
