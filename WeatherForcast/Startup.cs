using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMigration;
using Entity;
using Hangfire;
using Hangfire.SqlServer;
using Messenger.Client.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherForcast.Services;
using WeatherForcast.Services.Implementation;

namespace WeatherForcast
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IOptions<ConnectionStrings> options)
        {
            _options = options;
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment hostingEnvironment;
        public IOptions<ConnectionStrings> _options;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //mvc
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddControllersAsServices();
            //Meseger fb chat bot
            services.AddMessengerClient("EAAJUQJULnJgBAKZC6o88cNTeJHXNAIBgqhaQXuv2zLgFI6hTZAU15hb7c7Dvn4QiCquE1289xe5lyK7XtL6mwhbWIoT34yXzIPSGPPal81LafpZBPpvjtmPlU955j98GavtTNq3VrPc1l2s6ZCuKTlwiOxuRYIAGWYBhmZCIYmnNNax6l0AWmTRtMfaMmPioZD");
            //services.AddMessengerClient("EAAJUQJULnJgBABdZCj4THk1PbrI9n8eKOs7QWRoCUvaGpuac8LddZA0hb0ER0y4pUMNQVKNC32xWMFsAMi3nDnP7ZB5neocxrGv73OqGsZAyxj7skjseTrKZCBSZAjsSsTJawR9k0J0Q3bSkwPKs5QLI2kyPWnKNpC0Kxt4Ogxg5Sm0G5qGOdWAZC5HrcXLTSYZD");

            //auto injection
            //services.AddMessengerClient("%ACCESS_TOKEN%");
            services.AddScoped<IWeatherService, WeatherService>()
                    .AddScoped<IFbBotService, FbBotService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "655586308627608";
                facebookOptions.AppSecret = "0564032adac7e9560ffe3b64c54db88f";
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddDbContext<DataMigrationContext>();
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(@"Server=tcp:khoapiterrr99.database.windows.net,1433;Initial Catalog=weatherforcast;Persist Security Info=False;User ID=khoapiterrr;Password=linhcutehotmexinchao9(;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHangfireDashboard();

            //loggerFactory.AddConsole();
            app.UseSession();
            //app.UseHttpsRedirection();
            app.UseHangfireServer();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}