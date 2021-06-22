using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NerdStore.WebApp.MVC.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {

        }
    }
}
