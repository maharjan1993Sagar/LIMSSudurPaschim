﻿using LIMS.Core.Configuration;
using LIMS.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace LIMS.Framework.StartupConfigure
{
    /// <summary>
    /// Represents object for the configuring/load url rewrite rules from external file on application startup
    /// </summary>
    public class UrlRewriteStartup : ILIMSStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="webHostEnvironment">WebHostEnvironment</param>
        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            var serviceProvider = application.ApplicationServices;
            var LIMSConfig = serviceProvider.GetRequiredService<LIMSConfig>();
            var urlRewriteOptions = new RewriteOptions();
            var rewriteOptions = false;
            if (LIMSConfig.UseUrlRewrite)
            {
                if (File.Exists("App_Data/UrlRewrite.xml"))
                {
                    using (var streamReader = File.OpenText("App_Data/UrlRewrite.xml"))
                    {
                        rewriteOptions = true;
                        urlRewriteOptions.AddIISUrlRewrite(streamReader);
                    }
                }
            }
            if (LIMSConfig.UrlRewriteHttpsOptions)
            {
                rewriteOptions = true;
                urlRewriteOptions.AddRedirectToHttps(LIMSConfig.UrlRewriteHttpsOptionsStatusCode, LIMSConfig.UrlRewriteHttpsOptionsPort);
            }
            if (LIMSConfig.UrlRedirectToHttpsPermanent)
            {
                rewriteOptions = true;
                urlRewriteOptions.AddRedirectToHttpsPermanent();
            }
            if(rewriteOptions)
                application.UseRewriter(urlRewriteOptions);
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            get { return -50; }
        }
    }
}
