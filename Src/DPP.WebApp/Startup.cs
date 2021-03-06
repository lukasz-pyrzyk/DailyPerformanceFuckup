﻿using System;
using DPP.WebApp.Clients;
using DPP.WebApp.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DPP.WebApp
{
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
            var dbOptions = new DataStorageServiceOptions
            {
                CollectionName = "entries",
                DatabaseName = "telemetry",
                ApiKey = Environment.GetEnvironmentVariable("dpfapikey"),
                Endpoint = new Uri(Environment.GetEnvironmentVariable("dpfurl"))
            };

            services.AddHttpClient<StreamingClient>(x =>
            {
                x.BaseAddress = new Uri("https://github.com/");
            });

            services.AddSingleton(dbOptions);
            services.AddSingleton<DataStorageService>();
            services.AddSingleton<DataStorageServiceWithTcp>();
            services.AddTransient<SlowDataStorageService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
