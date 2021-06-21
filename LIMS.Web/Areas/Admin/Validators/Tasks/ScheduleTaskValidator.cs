using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Tasks;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Tasks
{
    public class ScheduleTaskValidator : BaseLIMSValidator<ScheduleTaskModel>
    {
        public ScheduleTaskValidator(
            IEnumerable<IValidatorConsumer<ScheduleTaskModel>> validators)
            : base(validators)
        {
            RuleFor(x => x.TimeInterval).GreaterThan(0).WithMessage("Time interval must be greater than zero");
        }
    }
}