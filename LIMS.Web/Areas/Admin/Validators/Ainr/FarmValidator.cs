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
    public class FarmValidator: BaseLIMSValidator<FarmModel>
    {
        public FarmValidator(IEnumerable<IValidatorConsumer<FarmModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.NameEnglish)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Farm.Name.Required"));
            RuleFor(x => x.District)
               .NotEmpty()
               .WithMessage(localizationService.GetResource("Admin.Farm.District.Required"));
            RuleFor(x => x.Category)
              .NotEmpty()
              .WithMessage(localizationService.GetResource("Admin.Farm.category.Required"));
            RuleFor(x => x.MoblileNo)
             .NotEmpty()
             .WithMessage(localizationService.GetResource("Admin.Farm.MobileNo.Required"));


        }

    }
}
