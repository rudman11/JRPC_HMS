
using DinkToPdf;
using DinkToPdf.Contracts;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using WebEssentials.AspNetCore.Pwa;

namespace JRPC_HMS
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), builder => builder.UseRowNumberForPaging()).EnableSensitiveDataLogging().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = false;    // уникальный email
                config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddSession();
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/");

                    options.Conventions.AllowAnonymousToPage("/Error");
                    options.Conventions.AllowAnonymousToPage("/Account/AccessDenied");
                    options.Conventions.AllowAnonymousToPage("/Account/ConfirmEmail");
                    options.Conventions.AllowAnonymousToPage("/Account/ExternalLogin");
                    options.Conventions.AllowAnonymousToPage("/Account/ForgotPassword");
                    options.Conventions.AllowAnonymousToPage("/Account/ForgotPasswordConfirmation");
                    options.Conventions.AllowAnonymousToPage("/Account/Lockout");
                    options.Conventions.AllowAnonymousToPage("/Account/Login");
                    options.Conventions.AllowAnonymousToPage("/Account/LoginWith2fa");
                    options.Conventions.AllowAnonymousToPage("/Account/LoginWithRecoveryCode");
                    options.Conventions.AllowAnonymousToPage("/Account/Register");
                    options.Conventions.AllowAnonymousToPage("/Account/ResetPassword");
                    options.Conventions.AllowAnonymousToPage("/Account/ResetPasswordConfirmation");
                    options.Conventions.AllowAnonymousToPage("/Account/SignedOut");
                    options.Conventions.AllowAnonymousToPage("/FBForm");
                    options.Conventions.AllowAnonymousToPage("/PostForm");
                    options.Conventions.AllowAnonymousToPage("/Service/api/Authenticate");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddProgressiveWebApp(new PwaOptions
            {
                AllowHttp = true,
                OfflineRoute = "/",
                BaseRoute = "/",
                RegisterServiceWorker = true,
                RoutesToPreCache = "/",
                Strategy = ServiceWorkerStrategy.CacheFirst
            });

            new CustomAssemblyLoadContext().LoadUnmanagedLibrary(string.Format("{0}/v0.12.4/64bit/libwkhtmltox.dll", _env.WebRootPath));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton(Configuration.GetSection("EmailSettings").Get<EmailSettings>());
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<Services.Profile.ProfileManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseStatusCodePagesWithReExecute("/Error/{0}");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            var supportedCultures = new[]
            {
               new CultureInfo("en-ZA"),

            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-ZA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();

        }
    }
}
