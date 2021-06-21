using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Core.Html;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Messages
{
    /// <summary>
    /// Contact attribute helper
    /// </summary>
    public partial class ContactAttributeFormatter : IContactAttributeFormatter
    {
        private readonly IWorkContext _workContext;
        private readonly IContactAttributeParser _contactAttributeParser;
        private readonly IDownloadService _downloadService;
        private readonly IWebHelper _webHelper;

        public ContactAttributeFormatter(IWorkContext workContext,
            IContactAttributeParser contactAttributeParser,
            IDownloadService downloadService,
            IWebHelper webHelper)
        {
            _workContext = workContext;
            _contactAttributeParser = contactAttributeParser;
            _downloadService = downloadService;
            _webHelper = webHelper;
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="customer">Customer</param>
        /// <param name="serapator">Serapator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <param name="allowHyperlinks">A value indicating whether to HTML hyperink tags could be rendered (if required)</param>
        /// <returns>Attributes</returns>
        public virtual async Task<string> FormatAttributes(string attributesXml,
            Customer customer,
            string serapator = "<br />",
            bool htmlEncode = true,
            bool allowHyperlinks = true)
        {
            var result = new StringBuilder();

            var attributes = await _contactAttributeParser.ParseContactAttributes(attributesXml);
            for (int i = 0; i < attributes.Count; i++)
            {
                var attribute = attributes[i];
                var valuesStr = _contactAttributeParser.ParseValues(attributesXml, attribute.Id);
                for (int j = 0; j < valuesStr.Count; j++)
                {
                    string valueStr = valuesStr[j];
                    string formattedAttribute = "";
                    if (!attribute.ShouldHaveValues())
                    {
                        //other attributes (textbox, datepicker)
                        formattedAttribute = string.Format("{0}: {1}", attribute.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id), valueStr);
                        //encode (if required)
                        if (htmlEncode)
                            formattedAttribute = WebUtility.HtmlEncode(formattedAttribute);
                    }
                    else
                    {
                        var attributeValue = attribute.ContactAttributeValues.Where(x => x.Id == valueStr).FirstOrDefault();
                        if (attributeValue != null)
                        {
                            formattedAttribute = string.Format("{0}: {1}", attribute.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id), attributeValue.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id));
                        }
                        //encode (if required)
                        if (htmlEncode)
                            formattedAttribute = WebUtility.HtmlEncode(formattedAttribute);
                    }

                    if (!String.IsNullOrEmpty(formattedAttribute))
                    {
                        if (i != 0 || j != 0)
                            result.Append(serapator);
                        result.Append(formattedAttribute);
                    }
                }
            }

            return result.ToString();
        }
    }
}
