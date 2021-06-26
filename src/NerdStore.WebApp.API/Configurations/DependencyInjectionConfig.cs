using Microsoft.Extensions.DependencyInjection;
using NerdStore.WebApp.API.Setup;
using System;

namespace NerdStore.WebApp.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            DependencyInjection.RegisterServices(services);
        }
    }
}
