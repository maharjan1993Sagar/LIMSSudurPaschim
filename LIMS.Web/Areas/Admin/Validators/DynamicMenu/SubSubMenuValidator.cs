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
    public class SubSubMenuValidater: BaseLIMSValidator<SubSubMenuModel>
    {
        public SubSubMenuValidater(
   IEnumerable<IValidatorConsumer<SubSubMenuModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.SubSubMenuName).NotEmpty().WithMessage(localizationService.GetResource("Admin.SubSubMenu.SubSubMenuName.Required"));
            RuleFor(x => x.SubMenuId).NotEmpty().WithMessage(localizationService.GetResource("Admin.SubSubMenu.SubMenu.Required"));
            RuleFor(x => x.SerialNo).NotNull().WithMessage(localizationService.GetResource("Admin.SubSubMenu.SerialNo.Required"))
                .GreaterThan(0).WithMessage(localizationService.GetResource("Admin.SubSubMenu.SerialNo.GreaterThanZero"));
        

        }
    }
}
