using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.PageContent
{
    public class PageContentValidater: BaseLIMSValidator<PageContentModel>
    {
        public PageContentValidater(
   IEnumerable<IValidatorConsumer<PageContentModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.PageContent.Title.Required"));
            RuleFor(x => x.PageName).NotEmpty().WithMessage(localizationService.GetResource("Admin.PageContent.PageName.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("Admin.PageContent.Description.Required"));
          
        }
    }
}
