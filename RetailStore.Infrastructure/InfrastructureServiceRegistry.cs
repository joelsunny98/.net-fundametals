using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetailStore.Contracts;
using RetailStore.Persistence;

namespace RetailStore.Infrastructure;

public static class InfrastructureServiceRegistry
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddPostgres(services, configuration);
        services.AddScoped<IRetailStoreDbContext>(provider => provider.GetService<RetailStoreDbContext>()!);
    }

    internal static void AddPostgres(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<RetailStoreDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
    }
}
