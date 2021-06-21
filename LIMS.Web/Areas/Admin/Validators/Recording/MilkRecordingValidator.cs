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
    public class MilkRecordingValidator:BaseLIMSValidator<MilkRecordingModel>
    {
        public MilkRecordingValidator(
         IEnumerable<IValidatorConsumer<MilkRecordingModel>> validators,
         ILocalizationService localizationService)
         : base(validators)
        {
            RuleFor(x => x.RecordingDate).NotEmpty().WithMessage(localizationService.GetResource("Admin.Recording.MilkRecording.RecordingDate.Required"));
            RuleFor(x => x.RecordingPeriod).NotEmpty().WithMessage(localizationService.GetResource("Admin.Recording.MilkRecording.RecordingPeriod.Required"));

        }
    }
}
