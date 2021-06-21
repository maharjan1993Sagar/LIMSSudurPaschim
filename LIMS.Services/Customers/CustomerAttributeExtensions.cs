using LIMS.Domain.Customers;

namespace LIMS.Services.Customers
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CustomerAttributeExtensions
    {
        /// <summary>
        /// A value indicating whether this customer attribute should have values
        /// </summary>
        /// <param name="customerAttribute">Customer attribute</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this CustomerAttribute customerAttribute)
        {
            if (customerAttribute == null)
                return false;

            //other attribute controle types support values
            return true;
        }
    }
}
