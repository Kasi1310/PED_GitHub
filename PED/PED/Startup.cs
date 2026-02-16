

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiseX.Data;
using WiseX.Models;
using WiseX.Services;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using WiseX.Controllers;
using WiseX.Helpers;
using Microsoft.AspNetCore.Authorization;
using WiseX.Handlers;
namespace WiseX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }


        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //options.EnableEndpointRouting = false; // Required for attribute routes in ASP.NET Core 2.x
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            //Session config
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".CodeComputer.Session";
                //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                //Added one more minute to delay the timeout to log session out time to DB
                options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToDouble(Configuration["AppSettings:SessionTimeOut"]) + 1);
                //options.IdleTimeout = TimeSpan.FromMinutes(20);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //DBContext
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DBConnection.ConnectionString, sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(120)));

            //Core Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Identity config
            services.Configure<IdentityOptions>(options =>
            {
                // We override the default so we can use our demo user
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMvc(
              config =>
              {
                  config.Filters.Add(typeof(CustomExceptionFilter));
              }
              );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MenuAccessPolicy", policy => policy.Requirements.Add(new MenuAccessRequirement("RoleID")));
            });
            services.AddTransient<IAuthorizationHandler, MenuAccessHandler>();
        }

        private void configuration(ApplicationInsightsServiceOptions obj)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                // app.UseExceptionHandler("/Home/Error");

                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
           
            app.UseStaticFiles();
            app.UseAuthentication();

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
