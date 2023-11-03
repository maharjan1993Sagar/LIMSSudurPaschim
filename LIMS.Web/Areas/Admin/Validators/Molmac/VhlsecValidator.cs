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
    public class VhlsecValidator : BaseLIMSValidator<VhlsecModel>
    {
        public VhlsecValidator(IEnumerable<IValidatorConsumer<VhlsecModel>> validators,
         ILocalizationService localizationService) : base(validators)
        {
            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vhlsec.Fields.EnglishName.Required"));
            RuleFor(x => x.DolfdId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vhlsec.Fields.Dolfd.Required"));
            RuleFor(x => x.Provience).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vhlsec.Fields.Province.Required"));
            RuleFor(x => x.District).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vhlsec.Fields.District.Required"));
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vhlsec.Fields.Email.Required"));
            RuleFor(x => x.UserNameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vhlsec.Fields.UserName.Required"));
        }
    }
}
