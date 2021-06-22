using Microsoft.Extensions.DependencyInjection;
using NerdStore.WebApp.MVC.Setup;
using System;

namespace NerdStore.WebApp.MVC.Configurations
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
