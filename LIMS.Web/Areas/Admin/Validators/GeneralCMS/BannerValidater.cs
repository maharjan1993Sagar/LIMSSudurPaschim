using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Banner
{
    public class BannerValidater: BaseLIMSValidator<BannerModel>
    {
        public BannerValidater(
   IEnumerable<IValidatorConsumer<BannerModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.Banner.Title.Required"));
            RuleFor(x => x.ImageModel.PictureId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Banner.Picture.Required"));
          
        }
    }
}
