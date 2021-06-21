using LIMS.Domain.Common;

namespace LIMS.Services.Common
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class AddressAttributeExtensions
    {
        /// <summary>
        /// A value indicating whether this address attribute should have values
        /// </summary>
        /// <param name="addressAttribute">Address attribute</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this AddressAttribute addressAttribute)
        {
            if (addressAttribute == null)
                return false;

            //other attribute controle types support values
            return true;
        }
    }
}
