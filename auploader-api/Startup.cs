﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auploader.api.Model;
using auploader.api.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace auploader.api
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
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc();
            //services.Configure<Settings>(sett => Configuration.GetSection("Settings").Bind(sett));
            services.Configure<S3Settings>(s3Sett => Configuration.GetSection("S3Settings").Bind(s3Sett));
            services.AddScoped<FileUploadService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                                   
            //app.UseCors(builder => builder.AllowAnyOrigin());
            //app.UseCors(builder => builder.WithOrigins("http://localhost:4200/*"));
            app.UseMvc();
        }
    }
}
