using LIMS.Domain.AInR;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LIMS.Services.Common
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface IPdfService
    {
        /// <summary>
        /// Print ear tags to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="earTags">Eartags</param>
        Task PrintEarTagsToPdf(Stream stream, IList<EarTag> earTags);       
    }
}