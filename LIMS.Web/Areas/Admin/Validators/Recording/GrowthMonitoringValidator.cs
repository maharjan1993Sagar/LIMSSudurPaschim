using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Validators.Recording
{
    public class GrowthMonitoringValidator:BaseLIMSValidator<GrowthMonitoringModel>
    {
        public GrowthMonitoringValidator(
        IEnumerable<IValidatorConsumer<GrowthMonitoringModel>> validators,
        ILocalizationService localizationService)
        : base(validators)
        {
            RuleFor(x => x.MonitoringDate).NotEmpty().WithMessage(localizationService.GetResource("Admin.Recording.MilkRecording.MonitoringDate.Required"));
            RuleFor(x => x.Weight).NotEmpty().WithMessage(localizationService.GetResource("Admin.Recording.MilkRecording.Weight.Required"));

        }
    }
}
