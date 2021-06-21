using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Domain.Organizations;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Molmac
{
    public class OrganizationValidator:BaseLIMSValidator<OrganizationModel>
    {
        public OrganizationValidator(IEnumerable<IValidatorConsumer<OrganizationModel>> validators,
        ILocalizationService localizationService) : base(validators)
        {

            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.EnglishName.Required"));
            RuleFor(x => x.Provience).NotEmpty().WithMessage(localizationService.GetResource("Admin.Nlbo.Fields.Province.Required"));
                 }
    }
}
