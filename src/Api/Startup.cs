using CleanTemplate.Api.Filters;
using CleanTemplate.Api.Settings;
using CleanTemplate.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using System;

namespace CleanTemplate.Api
{
    public class Startup
    {

        private readonly Logger _logger;
        private readonly AppSettings _appSettings;
        public Startup(IConfiguration configuration)
        {
            _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            _appSettings = new AppSettings();
            configuration.Bind(_appSettings);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.Info("Configuring services");

            //Create singleton from instance
            services.AddSingleton(_appSettings);

            _ConfigureNewtonsoftJson();

            services
                .AddControllers(options =>
                    options.Filters.Add(typeof(HttpResponseExceptionFilter))
                )
                .AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    $"v{_appSettings.Api.MajorVersion}", 
                    new OpenApiInfo { 
                        Title = $"{ _appSettings.Api.Name}",
                        Version = $"v{_appSettings.Api.MajorVersion}" 
                    }
                );
            });

            services.AddScoped<IPresenter, Presenter>();

            services.AddApplicationServices(_appSettings);
        }

        private void _ConfigureNewtonsoftJson()
        {
            var setting = new JsonSerializerSettings();
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() => {
                return setting;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _logger.Info("EnvironmentName={EnvironmentName}.", env.EnvironmentName);
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                _UseSwaggerUI(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void _UseSwaggerUI(IApplicationBuilder app)
        {
            string apiName =   _appSettings.Api.Name;
            var endpoint = _appSettings.SwaggerUI.FormatEndpoint(_appSettings.Api.Version);

            _logger.Info("UseSwaggerUI Endpoint={Endpoint}, Name={Name}", endpoint, apiName);
            app.UseSwaggerUI(c => c.SwaggerEndpoint(endpoint, apiName));
        }
    }
}
