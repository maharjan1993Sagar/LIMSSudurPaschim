using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.DynaicMenu
{
    public class MainMenuValidater: BaseLIMSValidator<MainMenuModel>
    {
        public MainMenuValidater(
   IEnumerable<IValidatorConsumer<MainMenuModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.MainMenuName).NotEmpty().WithMessage(localizationService.GetResource("Admin.MainMenu.MainMenuName.Required"));
            RuleFor(x => x.SerialNo).NotNull().WithMessage(localizationService.GetResource("Admin.MainMenu.SerialNo.Required"))
                .GreaterThan(0).WithMessage(localizationService.GetResource("Admin.MainMenu.SerialNo.GreaterThanZero"));


        }
    }
}
