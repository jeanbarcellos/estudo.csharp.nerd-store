using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Application.AutoMapper;
using System;

namespace NerdStore.WebApp.MVC.Configurations
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
        }
    }
}
