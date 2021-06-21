using FluentValidation;
using System.Collections.Generic;

namespace LIMS.Core.Validators
{
    public abstract class BaseLIMSValidator<T> : AbstractValidator<T> where T : class
    {

        protected BaseLIMSValidator(IEnumerable<IValidatorConsumer<T>> validators)
        {
            PostInitialize(validators);
        }

        protected virtual void PostInitialize(IEnumerable<IValidatorConsumer<T>> validators)
        {
            foreach (var item in validators)
            {
                item.AddRules(this);
            }

        }

    }


}