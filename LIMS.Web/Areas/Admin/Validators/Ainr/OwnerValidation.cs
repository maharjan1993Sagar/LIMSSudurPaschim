using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Ainr
{
    public class OwnerValidation:BaseLIMSValidator<OwnerModel>
    {
        public OwnerValidation(IEnumerable<IValidatorConsumer<OwnerModel>> validators,
          ILocalizationService localizationService)
          : base(validators)
        {

            RuleFor(x => x.NameEnglish)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Owner.Name.Required"));
            RuleFor(x => x.Type)
               .NotEmpty()
               .WithMessage(localizationService.GetResource("Admin.Owner.Type.Required"));

        }
    }
}
