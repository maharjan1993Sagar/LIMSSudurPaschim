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
    public class DolfdValidator : BaseLIMSValidator<DolfdModel>
    {
        public DolfdValidator(IEnumerable<IValidatorConsumer<DolfdModel>> validators,
         ILocalizationService localizationService) : base(validators)
        {
            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Dolfd.Fields.EnglishName.Required"));
            RuleFor(x => x.MoamacId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Dolfd.Fields.Molmac.Required"));
            RuleFor(x => x.Address).NotEmpty().WithMessage(localizationService.GetResource("Admin.Dolfd.Fields.Address.Required"));
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage(localizationService.GetResource("Admin.Dolfd.Fields.Email.Required"));
            RuleFor(x => x.UserNameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Dolfd.Fields.UserName.Required"));
        }
    }
}
