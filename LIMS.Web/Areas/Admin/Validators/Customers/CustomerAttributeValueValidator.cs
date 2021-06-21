using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Customers
{
    public class CustomerAttributeValueValidator : BaseLIMSValidator<CustomerAttributeValueModel>
    {
        public CustomerAttributeValueValidator(
            IEnumerable<IValidatorConsumer<CustomerAttributeValueModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerAttributes.Values.Fields.Name.Required"));
        }
    }
}