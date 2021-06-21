using FluentValidation;
using LIMS.Domain.Customers;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Common;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Common
{
    public class LoginValidator : BaseLIMSValidator<LoginModel>
    {
        public LoginValidator(
            IEnumerable<IValidatorConsumer<LoginModel>> validators,
            ILocalizationService localizationService, CustomerSettings customerSettings)
            : base(validators)
        {
            if (!customerSettings.UsernamesEnabled)
            {
                //login by email
                RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Login.Fields.Email.Required"));
                RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            }
        }
    }
}