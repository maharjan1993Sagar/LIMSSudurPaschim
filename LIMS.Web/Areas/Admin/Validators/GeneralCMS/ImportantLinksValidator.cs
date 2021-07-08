using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.ImportantLinks
{
    public class ImportantLinksValidater: BaseLIMSValidator<ImportantLinksModel>
    {
        public ImportantLinksValidater(
   IEnumerable<IValidatorConsumer<ImportantLinksModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.LinkName).NotEmpty().WithMessage(localizationService.GetResource("Admin.ImportantLinks.LinkName.Required"));
            RuleFor(x => x.URL).NotEmpty().WithMessage(localizationService.GetResource("Admin.ImportantLinks.URL.Required"));
            RuleFor(x => x.SerialNo).NotNull().WithMessage(localizationService.GetResource("Admin.ImportantLinks.SerialNo.Required"))
                                     .GreaterThan(0).WithMessage(localizationService.GetResource("Admin.ImportantLinks.SerialNo.GreaterThanZero"));


        }
    }
}
