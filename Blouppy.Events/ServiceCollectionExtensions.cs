using System.Reflection;
using Blouppy.Events.Abstractions;
using Blouppy.Events.EventPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blouppy.Events;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers handlers types from the specified assemblies
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="optionsAction">The action used to configure the options.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddBlouppyEvents(
        this IServiceCollection services, 
        Action<BlouppyEventsOptionsBuilder> optionsAction)
    {
        var serviceConfiguration = new BlouppyEventsOptionsBuilder();

        optionsAction.Invoke(serviceConfiguration);

        return services.AddBlouppyEvents(serviceConfiguration);
    }
    
    /// <summary>
    /// Registers handlers and mediator types from the specified assemblies
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="optionsBuilder">The configuration options.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddBlouppyEvents(
        this IServiceCollection services, 
        BlouppyEventsOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.AssembliesToScan.Count == 0)
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
        }
        
        services.AddScoped<IPublisher, Publisher>();

        // Use TryAdd, so any existing service registration doesn't get overridden
        services.TryAdd(new ServiceDescriptor(
            typeof(IEventPublisher), 
            optionsBuilder.EventPublisherType, 
            optionsBuilder.Lifetime));
        
        services.TryAddGenericImplementations(
            typeof(IEventHandler<>), 
            optionsBuilder.AssembliesToScan,
            lifetime: optionsBuilder.Lifetime);

        return services;
    }
   
    private static void TryAddGenericImplementations(
        this IServiceCollection services,
        Type interfaceType,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime)
    {
        foreach (var assembly in assemblies)
        {
            services.TryAddGenericImplementations(
                interfaceType, 
                assembly, 
                lifetime);
        }
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
