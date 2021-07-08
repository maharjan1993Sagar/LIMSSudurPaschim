using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.ContactUs
{
    public class ContactUsValidater: BaseLIMSValidator<ContactUsModel>
    {
        public ContactUsValidater(
   IEnumerable<IValidatorConsumer<ContactUsModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.OfficeName).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContactUs.OfficeName.Required"));
            RuleFor(x => x.Address).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContactUs.Address.Required"));
            RuleFor(x => x.Phone1).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContactUs.Address.Required"));
            RuleFor(x => x.Email1).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContactUs.Address.Required"));
          
        }
    }
}
