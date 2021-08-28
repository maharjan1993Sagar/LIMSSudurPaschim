using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LIMS.Framework.Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Autofac;
using Serilog;
using System;
using Microsoft.AspNetCore.Mvc.Razor;
using LIMS.Website1.Data;

namespace LIMS.Website1
{
    /// <summary>
    /// Represents startup class of application
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region wangkanai Detection
            //  services.AddDetection();

            // Needed by Wangkanai Detection
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            #endregion wangkanai Detection
            //Default calls
            services.AddControllersWithViews();
            services.AddRazorPages();
           // services.AddHttpContextAccessor();


            //Enable CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://google.com");
                    });
            });


            //Configuration for automapper (mapping)
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //var mapperConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});
            //IMapper mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(mapper);


            //Configuration for Hangfire
            //var options = new MongoStorageOptions {
            //    MigrationOptions = new MongoMigrationOptions {
            //        Strategy = MongoMigrationStrategy.Drop,
            //        BackupStrategy = MongoBackupStrategy.None
            //        // MigrationStrategy = new MigrateMongoMigrationStrategy(),
            //        // BackupStrategy = new CollectionMongoBackupStrategy()
            //    },
            //    Prefix = "hangfire.mongo",
            //    CheckConnection = true
            //};

            //services.AddHangfire(x => x.UseMongoStorage(
            //    Configuration.GetConnectionString("HangfireConnection"),
            //    options));


            //Default methods
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;

            //});

            ////Configuration for database context as well as services
            services.AddScoped<IGetResource, GetResource>();
            
            //// services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IAdminRepository, AdminRepository>();
            //services.AddScoped<IGroupRepository, GroupRepository>();
            //services.AddScoped<IGroupUserRepository, GroupUserRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IPhoneDetailRepository, PhoneDetailRepository>();
            //services.AddScoped<IShortMessageRepository, ShortMessageRepository>();
            //services.AddScoped<IGroupUserJoin, GroupUserJoin>();
            //services.AddScoped<IEmailSender, EmailSender>();
            //services.AddScoped<IHangfireJobs, HangfireJobs>();
            //services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            //services.AddScoped<IExcelServices, ExcelServices>();
            //services.AddScoped<IPermissionRepository, PermissionRepository>();
            //services.AddScoped<ICommitteeRepository, CommitteeRepository>();
            //services.AddScoped<IGroupAssignUserRepository, GroupAssignUserRepository>();
            //services.AddScoped<OSBrowserInfo>();
            //services.AddSingleton<Resources.LanguageService>();

            #region multilanguage            

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[] { "en-US", "ne-NP" };
            //    options.SetDefaultCulture(supportedCultures[0])
            //        .AddSupportedCultures(supportedCultures)
            //        .AddSupportedUICultures(supportedCultures);
            //});

            //services.AddMvc();
            //.AddViewLocalization()
            //.AddDataAnnotationsLocalization(options =>
            //{
            //    options.DataAnnotationLocalizerProvider = (type, factory) =>
            //    {

            //        var assemblyName = new AssemblyName(typeof(ShareResource).GetTypeInfo().Assembly.FullName);

            //        return factory.Create("ShareResource", assemblyName.Name);

            //    };

            //});



            //services.Configure<RequestLocalizationOptions>(
            //    options =>
            //    {
            //        var supportedCultures = new List<CultureInfo>
            //            {
            //                new CultureInfo("en-US"),
            //                new CultureInfo("ne-NP")
            //            };



            //        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

            //        options.SupportedCultures = supportedCultures;
            //        options.SupportedUICultures = supportedCultures;
            //        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

            //    });


            services.AddMvc()
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "ne-NP" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });


            #endregion


            //Authentication Configuration
            //services.Configure<CookieTempDataProviderOptions>(options =>
            //{
            //    options.Cookie.IsEssential = true;
            //});
            //services.AddAuthentication()
            //  .AddCookie("Cookies",
            //    options =>
            //    {
            //        options.Cookie.Name = "Admin";
            //        options.LoginPath = new PathString("/Auth/Login/");
            //        options.AccessDeniedPath = new PathString("/Home/accessdenied/");
            //        options.LogoutPath = new PathString("/Auth/Logout");
            //        options.SlidingExpiration = false;
            //    });


            ////Authorization Configuration
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin",
            //        policy =>
            //        {
            //            policy.RequireRole("Admin");
            //        });

            //    options.AddPolicy("SuperAdmin",
            //        policy =>
            //        {
            //            policy.RequireClaim("Role", "SuperAdmin");
            //        });

            //    options.AddPolicy("AddGroup",
            //        policy => policy.RequireClaim("PerGroup", "CanAddGroup"));

            //    options.AddPolicy("ImportMember",
            //        policy => policy.RequireClaim("PerMember", "CanImportMember"));

            //    options.AddPolicy("AddUser",
            //        policy => policy.RequireClaim("PerUser", "CanAddUser"));

            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //multilanguage configuration
           
            //var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            //app.UseRequestLocalization(locOptions.Value);

            //End of multilanguage configuration

            #region wangkanai Detection


            #endregion wangkanai Detection
            // app.UseDetection();

            #region multilanguage
            // var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            //  app.UseRequestLocalization(locOptions.Value);


            //var supportedCultures = new[] { "en-US", "ne-NP" };
            //var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            //    .AddSupportedCultures(supportedCultures)
            //    .AddSupportedUICultures(supportedCultures);

            ////app.UseRequestLocalization(localizationOptions);

            //app.UseRequestLocalization();

            #endregion multilanguage

            #region IpAddress
            //app.UseForwardedHeaders(new ForwardedHeadersOptions {
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            //     ForwardedHeaders.XForwardedProto
            //});
            #endregion


            //  app.UseMvc();
            app.UseHttpsRedirection();


            var supportedCultures = new[] { "en-US", "ne-NP" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);




            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseCookiePolicy();
            app.UseAuthorization();

            #region For wangkanai detection
            app.UseSession();
            #endregion

            #region hangfire

            //app.UseHangfireServer();

            ////Basic Authentication added to access the Hangfire Dashboard  
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions() {
            //    AppPath = null,
            //    DashboardTitle = "Hangfire Dashboard",
            //    Authorization = new[]{
            //    new HangfireCustomBasicAuthenticationFilter{
            //        User = Configuration.GetSection("HangfireCredentials:UserName").Value,
            //        Pass = Configuration.GetSection("HangfireCredentials:Password").Value
            //    }
            //},
            //});

            #endregion hangfire

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}


//    public class Startup
//    {
//        #region Properties

//        /// <summary>
//        /// Get configuration root of the application
//        /// </summary>
//        public IConfiguration Configuration { get; }

//        #endregion

//        #region Ctor

//        public Startup(IHostEnvironment environment)
//        {
//            //create configuration
//            Configuration = new ConfigurationBuilder()
//                .SetBasePath(environment.ContentRootPath)
//                .AddJsonFile("App_Data/appsettings.json", optional: false, reloadOnChange: true)
//                .AddEnvironmentVariables()
//                .Build();

//            //create logger
//            Log.Logger = new LoggerConfiguration()
//                .ReadFrom.Configuration(Configuration)
//            .CreateLogger();
//        }

//        #endregion

//        /// <summary>
//        /// Add services to the application and configure service provider
//        /// </summary>
//        /// <param name="services">Collection of service descriptors</param>
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.ConfigureApplicationServices(Configuration);
//        }

//        /// <summary>
//        /// Configure the application HTTP request pipeline
//        /// </summary>
//        /// <param name="application">Builder for configuring an application's request pipeline</param>
//        /// <param name="env">IWebHostEnvironment</param>
//        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
//        {
//            application.ConfigureRequestPipeline(webHostEnvironment);
//        }

//        /// <summary>
//        /// ConfigureContainer is where you can register things directly
//        /// with Autofac. This runs after ConfigureServices so the things
//        /// here will override registrations made in ConfigureServices.
//        /// </summary>
//        /// <param name="builder"></param>
//        public void ConfigureContainer(ContainerBuilder builder)
//        {
//            builder.ConfigureContainer(Configuration);
//        }
//    }
//}
