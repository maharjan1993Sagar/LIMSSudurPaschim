using FluentValidation;
using LIMS.Core.Validators;
using LIMS.Services.Localization;
using LIMS.Web.Areas.Admin.Models.Common;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Validators.Common
{
    public class ReviewValidator : BaseLIMSValidator<ReviewModel>
    {
        public ReviewValidator(
            IEnumerable<IValidatorConsumer<ReviewModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Review.Fields.Title.Required"));
            RuleFor(x => x.ReviewText)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Review.Fields.ReviewText.Required"));
        }
    }
}