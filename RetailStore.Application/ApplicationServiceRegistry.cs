using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RetailStore.Application;

public static class ApplicationServiceRegistry
{
    public static void AddApplicationServices(this IServiceCollection services) 
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
