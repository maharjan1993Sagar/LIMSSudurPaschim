using LIMS.Core.Configuration;
using LIMS.Core.Data;
using LIMS.Core.Infrastructure;
using LIMS.Framework.Infrastructure.Extensions;
using LIMS.Framework.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LIMS.Framework.StartupConfigure
{
    /// <summary>
    /// Represents object for the configuring authentication middleware on application startup
    /// </summary>
    public class AuthenticationStartup : ILIMSStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new LIMSConfig();
            configuration.GetSection("LIMS").Bind(config);

            //add data protection
            services.AddLIMSDataProtection(config);
            //add authentication
            services.AddLIMSAuthentication(configuration);
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            //check whether database is installed
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //configure authentication
            application.UseLIMSAuthentication();

            //set storecontext
            application.UseMiddleware<StoreContextMiddleware>();

            //set workcontext
            application.UseMiddleware<WorkContextMiddleware>();

        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //authentication should be loaded before MVC
            get { return 500; }
        }
    }
}
