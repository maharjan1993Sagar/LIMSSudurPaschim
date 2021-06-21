using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Customers
{
    public class CustomerTagValidator : BaseLIMSValidator<CustomerTagModel>
    {
        public CustomerTagValidator(
            IEnumerable<IValidatorConsumer<CustomerTagModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerTags.Fields.Name.Required"));
        }
    }
}