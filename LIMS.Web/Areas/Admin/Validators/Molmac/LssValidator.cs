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
    public class LssValidator : BaseLIMSValidator<LssModel>
    {
        public LssValidator(IEnumerable<IValidatorConsumer<LssModel>> validators,
        ILocalizationService localizationService) : base(validators)
        {
            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.EnglishName.Required"));
            RuleFor(x => x.VhlsecId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.Vhlsec.Required"));
            RuleFor(x => x.Provience).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.Province.Required"));
            RuleFor(x => x.District).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.District.Required"));
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.Email.Required"));
            RuleFor(x => x.UserNameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.UserName.Required"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizationService.GetResource("Admin.Lss.Fields.Password.Required"));
        }
    }
}
