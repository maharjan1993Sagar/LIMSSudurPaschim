using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Gallery
{
    public class GalleryValidater : BaseLIMSValidator<GalleryModel>
    {
        public GalleryValidater(
   IEnumerable<IValidatorConsumer<GalleryModel>> validators,
   ILocalizationService localizationService) : base(validators)
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage(localizationService.GetResource("Admin.Gallery.Type.Required"));
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.Gallery.Title.Required"));
        }
    }
}
