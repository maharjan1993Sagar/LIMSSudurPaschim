#! "netcoreapp3.1"
#r "LIMS.Core"
#r "LIMS.Framework"
#r "LIMS.Services"
#r "LIMS.Web"


using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Events;
using LIMS.Web.Models.Common;
using System.Threading.Tasks;
using System;

/* Sample code to validate ZIP Code field in the Address */
public class ZipCodeValidation : IValidatorConsumer<AddressModel>
{
    public void AddRules(BaseLIMSValidator<AddressModel> validator)
    {
        validator.RuleFor(x => x.ZipPostalCode).Matches(@"^[0-9]{2}\-[0-9]{3}$")
            .WithMessage("Provided zip code is invalid");
    }
}
