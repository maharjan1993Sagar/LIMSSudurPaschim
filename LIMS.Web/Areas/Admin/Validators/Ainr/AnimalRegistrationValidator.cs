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
    public class AnimalRegistrationValidator:BaseLIMSValidator<AnimalRegistrationModel>
    {
        public AnimalRegistrationValidator(IEnumerable<IValidatorConsumer<AnimalRegistrationModel>> validators,
           ILocalizationService localizationService)
           : base(validators)
        {
            RuleFor(x => x.SpeciesId)
              .NotEmpty()
              .WithMessage(localizationService.GetResource("Admin.AnimalRegistration.SpeciesId.Required"));
            RuleFor(x => x.DOB)
               .NotEmpty()
               .WithMessage(localizationService.GetResource("Admin.AnimalRegistration.Dob.Required"));
          
        }

       
    }
}
