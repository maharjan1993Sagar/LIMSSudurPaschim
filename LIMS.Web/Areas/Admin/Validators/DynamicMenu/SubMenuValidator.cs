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
    public class SubMenuValidater: BaseLIMSValidator<SubMenuModel>
    {
        public SubMenuValidater(
   IEnumerable<IValidatorConsumer<SubMenuModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.SubMenu.Name.Required"));
            RuleFor(x => x.MainMenuId).NotEmpty().WithMessage(localizationService.GetResource("Admin.SubMenu.MainMenu.Required"));
            RuleFor(x => x.SerialNo).NotNull().WithMessage(localizationService.GetResource("Admin.SubSubMenu.SerialNo.Required"))
                .GreaterThan(0).WithMessage(localizationService.GetResource("Admin.SubSubMenu.SerialNo.GreaterThanZero"));
        }
    }
}
