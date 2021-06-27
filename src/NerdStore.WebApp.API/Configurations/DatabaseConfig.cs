using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Data;
using NerdStore.Pagamentos.Data;
using NerdStore.Vendas.Data.Context;
using System;

namespace NerdStore.WebApp.API.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentException(nameof(configuration));

            services.AddDbContext<CatalogoContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<VendasContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PagamentosContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
