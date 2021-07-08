using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.NewsEvent
{
    public class NewsEventFile : SubBaseEntity
    {

        /// <summary>
        /// Gets or sets the farm identifier
        /// </summary>
        public string CMSEntityId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string PictureId { get; set; }
        public int DisplayOrder { get; set; }

        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. product name)
        /// </summary>
        public string AltAttribute { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. product name)
        /// </summary>
        public string TitleAttribute { get; set; }

        public string Type { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string OverrideTitleAttribute { get; set; }
        public string OverrideAltAttribute { get; set; }
        public string PictureUrl{ get; set; }

    }
}
