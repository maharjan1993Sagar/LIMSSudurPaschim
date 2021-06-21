using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    public partial interface ICustomerAttributeFormatter
    {
        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="serapator">Serapator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <returns>Attributes</returns>
        Task<string> FormatAttributes(string attributesXml, string serapator = "<br />", bool htmlEncode = true);
    }
}
