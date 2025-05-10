using System.Reflection;
using Blouppy.Events.Abstractions;
using Blouppy.Events.EventPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blouppy.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlouppyEvents(
        this IServiceCollection services, 
        params Assembly[] assembliesToScan)
    {
        services.AddScoped<IPublisher, Publisher>();

        // You can use replace ForeachAwaitPublisher by TaskWhenAllPublisher depending on your use case.
        services.AddScoped<IEventPublisher, ForeachAwaitPublisher>();

        // Register handlers in specified assemblies
        return services.TryAddGenericImplementations(
            typeof(IEventHandler<>), 
            assembliesToScan,
            lifetime: ServiceLifetime.Scoped);
    }
   
    private static IServiceCollection TryAddGenericImplementations(
        this IServiceCollection services,
        Type interfaceType,
        Assembly[] assemblies,
        ServiceLifetime lifetime)
    {
        foreach (var assembly in assemblies)
        {
            services.TryAddGenericImplementations(
                interfaceType, 
                assembly, 
                lifetime);
        }

        return services;
    }
    
    private static void TryAddGenericImplementations(
        this IServiceCollection services,
        Type interfaceType,
        Assembly assembly,
        ServiceLifetime lifetime)
    {
        var serviceDescriptors = assembly
            .GetTypes()
            .Where(typeFromAssembly => typeFromAssembly is
            {
                IsClass: true, 
                IsAbstract: false,
                IsGenericType: false
            })
            .SelectMany(typeFromAssembly => typeFromAssembly.GetInterfaces()
                .Where(interfaceFromAssembly => interfaceFromAssembly.IsGenericType 
                                                && interfaceFromAssembly.GetGenericTypeDefinition() == interfaceType)
                .Select(serviceType => new ServiceDescriptor(serviceType, typeFromAssembly, lifetime)));

        services.TryAddEnumerable(serviceDescriptors);
    }
}
