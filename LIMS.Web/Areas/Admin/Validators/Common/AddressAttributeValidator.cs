using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Common;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Common
{
    public class AddressAttributeValidator : BaseLIMSValidator<AddressAttributeModel>
    {
        public AddressAttributeValidator(
            IEnumerable<IValidatorConsumer<AddressAttributeModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Address.AddressAttributes.Fields.Name.Required"));
        }
    }
}