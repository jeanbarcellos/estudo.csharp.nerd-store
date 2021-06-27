using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NerdStore.WebApp.MVC.Configurations;

namespace NerdStore.WebApp.MVC
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
            // MVC Settings
            services.AddControllersWithViews();

            // Setting DBContexts
            services.AddDatabaseConfiguration(Configuration);

            // Em combina��o com UseDeveloperExceptionPage, isso captura exce��es relacionadas ao banco de dados que podem ser resolvidas usando Entity Framework migra��es. Quando essas exce��es ocorrem, uma resposta HTML com detalhes sobre poss�veis a��es para resolver o problema � gerada.
            services.AddDatabaseDeveloperPageExceptionFilter();

            // ASP.NET Identity Settings
            services.AddIdentityConfiguration(Configuration);

            // AutoMapper Settings
            services.AddAutoMapperConfiguration();

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            // .NET Native DI Abstraction
            services.AddDependencyInjectionConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Vitrine}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
