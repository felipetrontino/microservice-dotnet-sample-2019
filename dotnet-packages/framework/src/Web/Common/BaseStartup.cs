using Framework.Core.Config;
using Framework.Web.Extensions;
using Framework.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Framework.Web.Common
{
    public abstract class BaseStartup
    {
        private readonly IConfiguration _config;

        protected BaseStartup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    });
            });

            services.AddScoped<IConfiguration>(x => _config);
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            RegisterService(services, _config);

            services.AddMvc();

            services.AddSwaggerGen(x =>
            {
                x.OperationFilter<AuthorizationHeaderParameterOperationFilter>();

                x.CustomSchemaIds(t => t.FullName);
                x.SwaggerDoc("v1", new OpenApiInfo { Title = $"Blindando {Configuration.AppName.Get()} API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });

            app.UseAuthentication();

            app.UseGlobalErrorMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"./v1/swagger.json", $"API {Configuration.AppName.Get()} v1");
            });

            app.UseMvc();

            app.UseHealthChecks("/healthcheck");

            ConfigureApp(app, env, serviceProvider, applicationLifetime);
        }

        protected virtual void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IHostApplicationLifetime applicationLifetime)
        {
        }

        protected virtual void RegisterService(IServiceCollection services, IConfiguration config)
        {
        }
    }
}