using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.NewsEvent
{
    public class NewsEventTenderValidater: BaseLIMSValidator<NewsEventTenderModel>
    {
        public NewsEventTenderValidater(
   IEnumerable<IValidatorConsumer<NewsEventTenderModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
           RuleFor(x => x.Type).NotEmpty().WithMessage(localizationService.GetResource("Admin.NewsEvent.Type.Required"));
           RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.NewsEvent.Title.Required"));
        }
    }
}
