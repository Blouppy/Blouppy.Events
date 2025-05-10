using System.Reflection;
using Blouppy.Events.EventPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events;

public sealed class BlouppyEventsOptionsBuilder
{
    internal List<Assembly> AssembliesToScan { get; } = [];
    
    /// <summary>
    /// Type of event publisher strategy to register. Defaults to <see cref="ForeachAwaitEventPublisher"/>
    /// </summary>
    public Type EventPublisherType { get; set; } = typeof(ForeachAwaitEventPublisher);
    
    /// <summary>
    /// Service lifetime to register services. Default value is <see cref="ServiceLifetime.Scoped"/>
    /// </summary>
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;

    /// <summary>
    /// Register handlers from assemblies.
    /// </summary>
    /// <param name="assembliesToScan">Assemblies to scan</param>
    /// <returns>This</returns>
    public BlouppyEventsOptionsBuilder RegisterServicesFromAssemblies(
        params Assembly[] assembliesToScan)
    {
        AssembliesToScan.AddRange(assembliesToScan);

        return this;
    }
}
