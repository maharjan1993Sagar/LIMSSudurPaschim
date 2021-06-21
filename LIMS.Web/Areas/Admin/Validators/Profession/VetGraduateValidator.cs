using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using LIMS.Web.Areas.Admin.Models.Professionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Profession
{
    public class VetGratduateValidator : BaseLIMSValidator<VetGraduateModel>
    {
        public VetGratduateValidator(
           IEnumerable<IValidatorConsumer<VetGraduateModel>> validators,
           ILocalizationService localizationService)
           : base(validators)
        {
            RuleFor(x => x.NameEnglish).NotEmpty().WithMessage(localizationService.GetResource("Admin.Profession.VetGraduate.EnglishName.Required"));

        }
    }
}
