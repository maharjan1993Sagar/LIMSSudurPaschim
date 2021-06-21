using LIMS.Core;
using LIMS.Domain.Common;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.News;
using LIMS.Services.Helpers;
using LIMS.Services.Knowledgebase;
using LIMS.Services.Media;
using LIMS.Services.Topics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LIMS.Services.Seo
{
    /// <summary>
    /// Represents a sitemap generator
    /// </summary>
    public partial class SitemapGenerator : ISitemapGenerator
    {
        #region Constants

        private const string DateFormat = @"yyyy-MM-dd";

        /// <summary>
        /// At now each provided sitemap file must have no more than 50000 URLs
        /// </summary>
        private const int maxSitemapUrlNumber = 50000;

        #endregion

        #region Fields

       
        private readonly ITopicService _topicService;
        private readonly IKnowledgebaseService _knowledgebaseService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;
        private readonly CommonSettings _commonSettings;
        private readonly KnowledgebaseSettings _knowledgebaseSettings;
        private readonly NewsSettings _newsSettings;

        #endregion

        #region Ctor

        public SitemapGenerator(           
            ITopicService topicService,
            IPictureService pictureService,
            IKnowledgebaseService knowledgebaseService,
            IWebHelper webHelper,
            CommonSettings commonSettings,
            KnowledgebaseSettings knowledgebaseSettings,
            NewsSettings newsSettings)
        {
          
            _topicService = topicService;
            _pictureService = pictureService;
            _webHelper = webHelper;
            _commonSettings = commonSettings;
            _knowledgebaseService = knowledgebaseService;
            _knowledgebaseSettings = knowledgebaseSettings;
            _newsSettings = newsSettings;
        }

        #endregion

        #region Nested class

        /// <summary>
        /// Represents sitemap URL entry
        /// </summary>
        protected class SitemapUrl
        {
            public SitemapUrl(string location, string image, UpdateFrequency frequency, DateTime updatedOn)
            {
                Location = location;
                Image = image;
                UpdateFrequency = frequency;
                UpdatedOn = updatedOn;
            }

            /// <summary>
            /// Gets or sets URL of the page
            /// </summary>
            public string Location { get; set; }

            /// <summary>
            /// Gets or sets URL of the image
            /// </summary>
            public string Image { get; set; }

            /// <summary>
            /// Gets or sets a value indicating how frequently the page is likely to change
            /// </summary>
            public UpdateFrequency UpdateFrequency { get; set; }

            /// <summary>
            /// Gets or sets the date of last modification of the file
            /// </summary>
            public DateTime UpdatedOn { get; set; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get HTTP protocol
        /// </summary>
        /// <returns>Protocol name as string</returns>
        protected virtual string GetHttpProtocol()
        {
            return _webHelper.IsCurrentConnectionSecured() ? "https" : "http";
        }

        /// <summary>
        /// Generate URLs for the sitemap
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <returns>List of URL for the sitemap</returns>
        protected virtual async Task<IList<SitemapUrl>> GenerateUrls(IUrlHelper urlHelper, string language, string store)
        {
            var sitemapUrls = new List<SitemapUrl>();

            //home page
            var homePageUrl = urlHelper.RouteUrl("HomePage", null, GetHttpProtocol());
            sitemapUrls.Add(new SitemapUrl(homePageUrl, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));

            //search products
            var productSearchUrl = urlHelper.RouteUrl("ProductSearch", null, GetHttpProtocol());
            sitemapUrls.Add(new SitemapUrl(productSearchUrl, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));

            //contact us
            var contactUsUrl = urlHelper.RouteUrl("ContactUs", null, GetHttpProtocol());
            sitemapUrls.Add(new SitemapUrl(contactUsUrl, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));

            //news
            if (_newsSettings.Enabled)
            {
                var url = urlHelper.RouteUrl("NewsArchive", null, GetHttpProtocol());
                sitemapUrls.Add(new SitemapUrl(url, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));
            }

            //knowledgebase
            if (_knowledgebaseSettings.Enabled)
            {
                var url = urlHelper.RouteUrl("Knowledgebase", null, GetHttpProtocol());
                sitemapUrls.Add(new SitemapUrl(url, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));
            }
            
            //topics
            sitemapUrls.AddRange(await GetTopicUrls(urlHelper, language, store));

            //knowledgebase articles
            sitemapUrls.AddRange(await GetKnowledgebaseUrls(urlHelper, language));

            //custom URLs
            sitemapUrls.AddRange(GetCustomUrls());

            return sitemapUrls;
        }

        /// <summary>
        /// Get topic URLs for the sitemap
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <returns>Collection of sitemap URLs</returns>
        protected virtual async Task<IEnumerable<SitemapUrl>> GetTopicUrls(IUrlHelper urlHelper, string language, string store)
        {
            var topics = await _topicService.GetAllTopics(storeId: store);
            return topics.Where(t => t.IncludeInSitemap).Select(topic =>
            {
                var url = urlHelper.RouteUrl("Topic", new { SeName = topic.GetSeName(language) }, GetHttpProtocol());
                return new SitemapUrl(url, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow);
            });
        }

        /// <summary>
        /// Get knowledgebase articles URLs for the sitemap
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <returns>Collection of sitemap URLs</returns>
        protected virtual async Task<IEnumerable<SitemapUrl>> GetKnowledgebaseUrls(IUrlHelper urlHelper, string language)
        {
            var knowledgebasearticles = await _knowledgebaseService.GetPublicKnowledgebaseArticles();
            var knowledgebase = new List<SitemapUrl>();
            foreach (var knowledgebasearticle in knowledgebasearticles)
            {
                var url = urlHelper.RouteUrl("KnowledgebaseArticle", new { SeName = knowledgebasearticle.GetSeName(language) }, GetHttpProtocol());
                knowledgebase.Add(new SitemapUrl(url, string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));
            }

            return knowledgebase;
        }

        /// <summary>
        /// Get custom URLs for the sitemap
        /// </summary>
        /// <returns>Collection of sitemap URLs</returns>
        protected virtual IEnumerable<SitemapUrl> GetCustomUrls()
        {
            var storeLocation = _webHelper.GetStoreLocation();

            return _commonSettings.SitemapCustomUrls.Select(customUrl =>
                new SitemapUrl(string.Concat(storeLocation, customUrl), string.Empty, UpdateFrequency.Weekly, DateTime.UtcNow));
        }

        /// <summary>
        /// Write sitemap index file into the stream
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <param name="stream">Stream</param>
        /// <param name="sitemapNumber">The number of sitemaps</param>
        protected virtual async Task WriteSitemapIndex(IUrlHelper urlHelper, Stream stream, int sitemapNumber)
        {
            var xwSettings = new XmlWriterSettings {
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                Encoding = Encoding.UTF8,
                Async = true
            };

            using (var writer = XmlWriter.Create(stream, xwSettings))
            {
                await writer.WriteStartDocumentAsync();
                writer.WriteStartElement("sitemapindex");
                writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

                //write URLs of all available sitemaps
                for (var id = 1; id <= sitemapNumber; id++)
                {
                    var url = urlHelper.RouteUrl("sitemap-indexed.xml", new { Id = id }, GetHttpProtocol());
                    var location = XmlHelper.XmlEncode(url);

                    writer.WriteStartElement("sitemap");
                    writer.WriteElementString("loc", location);
                    writer.WriteElementString("lastmod", DateTime.UtcNow.ToString(DateFormat));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                await writer.FlushAsync();
            }
        }

        /// <summary>
        /// Write sitemap file into the stream
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <param name="stream">Stream</param>
        /// <param name="sitemapUrls">List of sitemap URLs</param>
        protected virtual async Task WriteSitemap(IUrlHelper urlHelper, Stream stream, IList<SitemapUrl> sitemapUrls)
        {
            var xwSettings = new XmlWriterSettings {
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                Encoding = Encoding.UTF8,
                Async = true
            };

            using (var writer = XmlWriter.Create(stream, xwSettings))
            {
                await writer.WriteStartDocumentAsync();
                writer.WriteStartElement("urlset");
                await writer.WriteAttributeStringAsync("urlset", "xmlns", null, "http://www.sitemaps.org/schemas/sitemap/0.9");

                if (_commonSettings.SitemapIncludeImage)
                    await writer.WriteAttributeStringAsync("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");

                await writer.WriteAttributeStringAsync("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

                //write URLs from list to the sitemap
                foreach (var url in sitemapUrls)
                {
                    writer.WriteStartElement("url");
                    var location = XmlHelper.XmlEncode(url.Location);
                    writer.WriteElementString("loc", location);

                    if (_commonSettings.SitemapIncludeImage && !string.IsNullOrEmpty(url.Image))
                    {
                        writer.WriteStartElement("image", "image", null);
                        writer.WriteElementString("image", "loc", null, url.Image);
                        writer.WriteEndElement();
                    }

                    writer.WriteElementString("changefreq", url.UpdateFrequency.ToString().ToLowerInvariant());
                    writer.WriteElementString("lastmod", url.UpdatedOn.ToString(DateFormat));
                    writer.WriteEndElement();
                }

                await writer.WriteEndElementAsync();
                await writer.FlushAsync();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This will build an xml sitemap for better index with search engines.
        /// See http://en.wikipedia.org/wiki/Sitemaps for more information.
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <param name="id">Sitemap identifier</param>
        /// <param name="language">Lang ident</param>
        /// <param name="store">Store ident</param>
        /// <returns>Sitemap.xml as string</returns>
        public virtual async Task<string> Generate(IUrlHelper urlHelper, int? id, string language, string store)
        {
            using (var stream = new MemoryStream())
            {
                await Generate(urlHelper, stream, id, language, store);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// This will build an xml sitemap for better index with search engines.
        /// See http://en.wikipedia.org/wiki/Sitemaps for more information.
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        /// <param name="stream">Stream of sitemap.</param>
        /// <param name="id">Sitemap identifier</param>
        /// <param name="language">Lang ident</param>
        /// <param name="store">Store ident</param>
        public virtual async Task Generate(IUrlHelper urlHelper, Stream stream, int? id, string language, string store)
        {
            //generate all URLs for the sitemap
            var sitemapUrls = await GenerateUrls(urlHelper, language, store);

            //split URLs into separate lists based on the max size 
            var sitemaps = sitemapUrls.Select((url, index) => new { Index = index, Value = url })
                .GroupBy(group => group.Index / maxSitemapUrlNumber).Select(group => group.Select(url => url.Value).ToList()).ToList();

            if (!sitemaps.Any())
                return;

            if (id.HasValue)
            {
                //requested sitemap does not exist
                if (id.Value == 0 || id.Value > sitemaps.Count)
                    return;

                //otherwise write a certain numbered sitemap file into the stream
                await WriteSitemap(urlHelper, stream, sitemaps.ElementAt(id.Value - 1));

            }
            else
            {
                //URLs more than the maximum allowable, so generate a sitemap index file
                if (sitemapUrls.Count >= maxSitemapUrlNumber)
                {
                    //write a sitemap index file into the stream
                    await WriteSitemapIndex(urlHelper, stream, sitemaps.Count);
                }
                else
                {
                    //otherwise generate a standard sitemap
                    await WriteSitemap(urlHelper, stream, sitemaps.First());
                }
            }
        }

        #endregion
    }
}