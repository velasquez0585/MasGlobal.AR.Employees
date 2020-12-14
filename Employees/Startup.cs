using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Employees
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("es-AR");
            Configuration = configuration;
            Environment = env;           
            this._loggerFactory = loggerFactory;
        }

        public IHostingEnvironment Environment { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            
            //config the db connection string 
            if (!Environment.IsDevelopment())
            {

            }


            services.AddTransient<DataInitializer, DataInitializer>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<UserWrapper, UserWrapper>();


            services.AddSingleton<IConfiguration>(Configuration);
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            
            services.AddAuthentication(options =>
            {               
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;                
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;                
                
            }).AddCookie(options => {
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Home/Login";
                
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                
            }).AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AllowAnonymousToPage("/Home/Login");
            }).AddSessionStateTempDataProvider();
            
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "EmployeesBO";
                options.Cookie.IsEssential = true;
            });


            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            DataInitializer data,UserWrapper userWrapper)
        {
            if (env.IsProduction())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
            });
            
            
        }
        
        
        private static string GetUserTempPath()
        {
            string path = Path.GetTempPath();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = "/tmp/";
            }
            return path;
        }
    }
}
