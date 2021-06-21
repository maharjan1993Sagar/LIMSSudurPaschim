using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.NewsEvent
{
    public  class NewsEventFile:SubBaseEntity
    {
        public NewsEventFile()
        {
            PictureId = Guid.NewGuid();
        }
        /// <summary>
        /// Gets or sets the farm identifier
        /// </summary>
        public string NewsEventTenderId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public Guid PictureId { get; set; }

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
        public string  FilePath { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }

    }
}
