﻿using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Directory;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Directory
{
    public class CountryValidator : BaseLIMSValidator<CountryModel>
    {
        public CountryValidator(
            IEnumerable<IValidatorConsumer<CountryModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.Name.Required"));

            RuleFor(x => x.TwoLetterIsoCode)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.TwoLetterIsoCode.Required"));
            RuleFor(x => x.TwoLetterIsoCode)
                .Length(2)
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.TwoLetterIsoCode.Length"));

            RuleFor(x => x.ThreeLetterIsoCode)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode.Required"));
            RuleFor(x => x.ThreeLetterIsoCode)
                .Length(3)
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode.Length"));
        }
    }
}