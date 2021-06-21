using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Customers;
using LIMS.Web.Areas.Admin.Validators.Common;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Customers
{
    public class CustomerAddressValidator : BaseLIMSValidator<CustomerAddressModel>
    {
        public CustomerAddressValidator(
            IEnumerable<IValidatorConsumer<CustomerAddressModel>> validators,
            IEnumerable<IValidatorConsumer<Models.Common.AddressModel>> addressvalidators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Address).SetValidator(new AddressValidator(addressvalidators, localizationService));
        }
    }
}
