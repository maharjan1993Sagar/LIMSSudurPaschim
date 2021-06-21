namespace LIMS.Core.Validators
{
    public interface IValidatorConsumer<T> where T : class
    {
        void AddRules(BaseLIMSValidator<T> validator);
    }
}
