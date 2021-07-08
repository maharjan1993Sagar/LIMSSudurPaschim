using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Employee
{
    public class EmployeeValidater: BaseLIMSValidator<EmployeeModel>
    {
        public EmployeeValidater(
   IEnumerable<IValidatorConsumer<EmployeeModel>> validators,
   ILocalizationService localizationService): base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Employee.Name.Required"));
            RuleFor(x => x.Designation).NotEmpty().WithMessage(localizationService.GetResource("Admin.Employee.Picture.Required"));
            RuleFor(x => x.Type).NotEmpty().WithMessage(localizationService.GetResource("Admin.Employee.Type.Required"));
            RuleFor(x => x.SerialNo).NotNull().WithMessage(localizationService.GetResource("Admin.Employee.SerialNo.Required"))
                 .GreaterThan(0).WithMessage(localizationService.GetResource("Admin.Employee.SerialNo.GreaterThanZero"));

        }
    }
}
