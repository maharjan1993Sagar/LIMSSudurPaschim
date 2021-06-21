using LIMS.Domain.Messages;

namespace LIMS.Services.Messages
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class ContactAttributeExtensions
    {
        /// <summary>
        /// Gets a value indicating whether this contact attribute should have values
        /// </summary>
        /// <param name="contactAttribute">Contact attribute</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this ContactAttribute contactAttribute)
        {
            if (contactAttribute == null)
                return false;
            
            //other attribute controle types support values
            return true;
        }

        /// <summary>
        /// A value indicating whether this contact attribute can be used as condition for some other attribute
        /// </summary>
        /// <param name="contactAttribute">Contact attribute</param>
        /// <returns>Result</returns>
        public static bool CanBeUsedAsCondition(this ContactAttribute contactAttribute)
        {
            if (contactAttribute == null)
                return false;

            //other attribute controle types support it
            return true;
        }
    }
}
