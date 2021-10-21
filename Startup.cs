using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using TodoList.Repository.Base;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace TodoList
{
    public class Startup
    {
        public IWebHostEnvironment env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ConfigureLogging(env);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<AppDbContext>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoList", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void ConfigureLogging(IWebHostEnvironment env)
        {
            var environment = env.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(environment, optional: false, reloadOnChange: true)
                .AddJsonFile(
                    environment,
                    optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration) =>
           new(new Uri(configuration["KibanaConfiguration:Uri"]))
           {
               AutoRegisterTemplate = true,
               IndexFormat = configuration["KibanaConfiguration:ApplicationName"]
           };
    }
}
