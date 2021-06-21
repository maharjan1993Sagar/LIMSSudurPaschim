using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AInR
{
    /// <summary>
    /// Represents a farm picture mapping
    /// </summary>
    public partial class FarmPicture : SubBaseEntity
    {
        /// <summary>
        /// Gets or sets the farm identifier
        /// </summary>
        public string FarmId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
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
    }
}
