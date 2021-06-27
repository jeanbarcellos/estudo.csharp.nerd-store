using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NerdStore.WebApp.API.Configurations;

namespace NerdStore.WebApp.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // WebAPI Settings
            services.AddControllers();

            // DBContexts Settings
            services.AddDatabaseConfiguration(Configuration);

            // ASP.NET Identity Settings & JWT
            services.AddIdentityConfiguration(Configuration);

            // AutoMapper Settings
            services.AddAutoMapperConfiguration();

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            // DI Abstraction
            services.AddDependencyInjectionConfiguration();

            // Swagger Settings
            services.AddSwaggerConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerSetup();
        }
    }
}
