﻿using FluentValidation.AspNetCore;
using LIMS.Core;
using LIMS.Core.Configuration;
using LIMS.Core.Data;
using LIMS.Core.Infrastructure;
using LIMS.Core.Plugins;
using LIMS.Domain.Configuration;
using LIMS.Framework.Extensions;
using LIMS.Core.ModelBinding;
using LIMS.Framework.Mvc.Routing;
using LIMS.Framework.Themes;
using LIMS.Services.Authentication;
using LIMS.Services.Authentication.External;
using LIMS.Services.Configuration;
using LIMS.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.AspNetCore3;
using Wkhtmltopdf.NetCore;

namespace LIMS.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Configured service provider</returns>
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //add LIMSConfig configuration parameters
            services.ConfigureStartupConfig<LIMSConfig>(configuration.GetSection("LIMS"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add api configuration parameters
            services.ConfigureStartupConfig<ApiConfig>(configuration.GetSection("Api"));

            //add accessor to HttpContext
            services.AddHttpContextAccessor();
            //add wkhtmltopdf
            services.AddWkhtmltopdf();

            //initialize and configure the engine
            Engine.Initialize(services, configuration);
            Engine.ConfigureServices(services, configuration);

        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services, LIMSConfig config)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie = new CookieBuilder() {
                    Name = $"{config.CookiePrefix}Antiforgery"
                };
                if (DataSettingsHelper.DatabaseIsInstalled())
                {
                    //whether to allow the use of anti-forgery cookies from SSL protected page on the other store pages which are not
                    options.Cookie.SecurePolicy = config.CookieSecurePolicyAlways ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;

                }
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services, LIMSConfig config)
        {
            services.AddSession(options =>
            {
                options.Cookie = new CookieBuilder() {
                    Name = $"{config.CookiePrefix}Session",
                    HttpOnly = true,
                };
                if (DataSettingsHelper.DatabaseIsInstalled())
                {
                    options.Cookie.SecurePolicy = config.CookieSecurePolicyAlways ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
                }
            });
        }

        /// <summary>
        /// Adds services required for themes support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddThemes(this IServiceCollection services)
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //themes support
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });
        }

        /// <summary>
        /// Adds data protection services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddLIMSDataProtection(this IServiceCollection services, LIMSConfig config)
        {
            if (config.PersistKeysToRedis)
            {
                services.AddDataProtection(opt => opt.ApplicationDiscriminator = "LIMS")
                    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(config.PersistKeysToRedisUrl));
            }
            else
            {
                var dataProtectionKeysPath = CommonHelper.MapPath("~/App_Data/DataProtectionKeys");
                var dataProtectionKeysFolder = new DirectoryInfo(dataProtectionKeysPath);
                //configure the data protection system to persist keys to the specified directory
                services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
            }
        }

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddLIMSAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new LIMSConfig();
            configuration.GetSection("LIMS").Bind(config);

            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = LIMSCookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = LIMSCookieAuthenticationDefaults.ExternalAuthenticationScheme;
            });

            //add main cookie authentication
            authenticationBuilder.AddCookie(LIMSCookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = config.CookiePrefix + LIMSCookieAuthenticationDefaults.AuthenticationScheme;
                options.Cookie.HttpOnly = true;
                options.LoginPath = LIMSCookieAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = LIMSCookieAuthenticationDefaults.AccessDeniedPath;

                options.Cookie.SecurePolicy = config.CookieSecurePolicyAlways ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
            });

            //add external authentication
            authenticationBuilder.AddCookie(LIMSCookieAuthenticationDefaults.ExternalAuthenticationScheme, options =>
            {
                options.Cookie.Name = config.CookiePrefix + LIMSCookieAuthenticationDefaults.ExternalAuthenticationScheme;
                options.Cookie.HttpOnly = true;
                options.LoginPath = LIMSCookieAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = LIMSCookieAuthenticationDefaults.AccessDeniedPath;
                options.Cookie.SecurePolicy = config.CookieSecurePolicyAlways ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
            });

            //register external authentication plugins now
            var typeFinder = new WebAppTypeFinder();
            var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
            //create and sort instances of external authentication configurations
            var externalAuthInstances = externalAuthConfigurations
                .Where(x => PluginManager.FindPlugin(x)?.Installed ?? true) //ignore not installed plugins
                .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x))
                .OrderBy(x => x.Order);

            //configure services
            foreach (var instance in externalAuthInstances)
                instance.Configure(authenticationBuilder, configuration);

        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddLIMSMvc(this IServiceCollection services, IConfiguration configuration)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddMvc(options =>
            {
                //add custom display metadata provider
                options.ModelMetadataDetailsProviders.Add(new LIMSMetadataProvider());
                //for API - ignore for PWA
                options.Conventions.Add(new ApiExplorerIgnores());
            });

            mvcBuilder.AddRazorRuntimeCompilation();

            var config = new LIMSConfig();
            configuration.GetSection("LIMS").Bind(config);

            //set compatibility version
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            if (config.UseHsts)
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                });
            }

            if (config.UseHttpsRedirection)
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = config.HttpsRedirectionRedirect;
                    options.HttpsPort = config.HttpsRedirectionHttpsPort;
                });
            }
            //use session-based temp data provider
            if (config.UseSessionStateTempDataProvider)
            {
                mvcBuilder.AddSessionStateTempDataProvider();
            }

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //add fluent validation
            var typeFinder = new WebAppTypeFinder();

            mvcBuilder.AddFluentValidation(configuration =>
            {
                var assemblies = typeFinder.GetAssemblies();
                configuration.RegisterValidatorsFromAssemblies(assemblies);
                configuration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            return mvcBuilder;
        }

        /// <summary>
        /// Add mini profiler service for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddLIMSMiniProfiler(this IServiceCollection services)
        {
            //whether database is already installed
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //add MiniProfiler services
            services.AddMiniProfiler(options =>
            {
                options.IgnoredPaths.Add("/api");
                options.IgnoredPaths.Add("/odata");
                options.IgnoredPaths.Add("/health/live");
                options.IgnoredPaths.Add("/.well-known/pki-validation");
                //determine who can access the MiniProfiler results
                options.ResultsAuthorize = request =>
                    !request.HttpContext.RequestServices.GetRequiredService<LIMSConfig>().DisplayMiniProfilerInPublicStore ||
                    request.HttpContext.RequestServices.GetRequiredService<IPermissionService>().Authorize(StandardPermissionProvider.AccessAdminPanel).Result;
            });
        }

        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddLIMSRedirectResultExecutor(this IServiceCollection services)
        {
            //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
            services.AddSingleton<IActionResultExecutor<RedirectResult>, LIMSRedirectResultExecutor>();
        }

        public static void AddSettings(this IServiceCollection services)
        {
            var typeFinder = new WebAppTypeFinder();
            var settings = typeFinder.FindClassesOfType<ISettings>();
            var instances = settings.Select(x => (ISettings)Activator.CreateInstance(x));
            foreach (var item in instances)
            {
                services.AddScoped(item.GetType(), (x) =>
                {
                    var type = item.GetType();
                    var storeId = string.Empty;
                    var settingService = x.GetRequiredService<ISettingService>();
                    var storeContext = x.GetRequiredService<IStoreContext>();
                    if (storeContext.CurrentStore == null)
                        storeId = ""; //storeContext.SetCurrentStore().Result.Id;
                    else
                        storeId = storeContext.CurrentStore.Id;

                    return settingService.LoadSetting(type, storeId);
                });
            }
        }

        public static void AddLIMSHealthChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
            hcBuilder.AddMongoDb(DataSettingsHelper.ConnectionString(),
                   name: "mongodb-check",
                   tags: new string[] { "mongodb" });
        }

        public static void AddHtmlMinification(this IServiceCollection services)
        {
            // Add WebMarkupMin services.
            services.AddWebMarkupMin(options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
            })
            .AddHtmlMinification(options =>
            {
                options.ExcludedPages = new List<IUrlMatcher> {
                    new WildcardUrlMatcher("/swagger/*"),
                    new WildcardUrlMatcher("/admin/*"),
                    new ExactUrlMatcher("/admin"),
                };
            })
            .AddXmlMinification(options =>
            {
                options.ExcludedPages = new List<IUrlMatcher> {
                    new WildcardUrlMatcher("/swagger/*"),
                    new WildcardUrlMatcher("/admin/*"),
                    new ExactUrlMatcher("/admin"),
                };
            })
            .AddHttpCompression(options => {
                options.ExcludedPages = new List<IUrlMatcher> {
                    new WildcardUrlMatcher("/swagger/*"),
                };
            });

        }

        /// <summary>
        /// Adds services for WebEncoderOptions
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddWebEncoder(this IServiceCollection services)
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        /// <summary>
        /// Adds services for mediatR
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMediator(this IServiceCollection services)
        {
            var typeFinder = new WebAppTypeFinder();
            var assemblies = typeFinder.GetAssemblies();
            services.AddMediatR(assemblies.ToArray());
        }

        /// <summary>
        /// Adds services for detection device
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDetectionDevice(this IServiceCollection services)
        {
            services.AddDetectionCore().AddDevice().AddCrawler();
        }


        /// <summary>
        /// Add Progressive Web App
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddPWA(this IServiceCollection services, IConfiguration configuration)
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var config = new LIMSConfig();
            configuration.GetSection("LIMS").Bind(config);
            if (config.EnableProgressiveWebApp)
            {
                var options = new WebEssentials.AspNetCore.Pwa.PwaOptions {
                    Strategy = (WebEssentials.AspNetCore.Pwa.ServiceWorkerStrategy)config.ServiceWorkerStrategy
                };
                services.AddProgressiveWebApp(options);
            }
        }
    }
}