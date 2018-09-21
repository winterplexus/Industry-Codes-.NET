//
//  Startup.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IndustryCodes.Models;
using IndustryCodes.Utility;

namespace IndustryCodes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            var sessionIdleTimeout = Assistant.GetFloatingPointValue(Configuration["SessionIdleTimeout"]);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(sessionIdleTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".IndustryCodes.Session";
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Configuration.GetConnectionString("IndustryCodesDatabase");

            services.AddDbContext<IndustryCodesContext>(options => options.UseSqlServer(connectionString));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }

            var staticFileCacheAge = Configuration["StaticFileCacheAge"];

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={staticFileCacheAge}");
                }
            });

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvcWithDefaultRoute();

            var logLevel = Configuration.GetSection("Logging:LogLevel").GetValue<LogLevel>("Default");

            loggerFactory.AddDebug(logLevel);
        }
    }
}