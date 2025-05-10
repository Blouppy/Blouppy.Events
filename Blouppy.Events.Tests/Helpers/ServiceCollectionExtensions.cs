using Blouppy.Events.Abstractions;
using Blouppy.Events.EventPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events.Tests.Helpers;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventPublisher<T>(this IServiceCollection services)
        where T : class, IEventPublisher 
        => services.AddScoped<IEventPublisher, T>();

    public static IServiceCollection AddHandler<TEvent>(this IServiceCollection services, IEventHandler<TEvent> handler)
        where TEvent : class, IEvent
        => services.AddScoped(_ => handler);

    public static IServiceCollection AddMockedEventHandler<TEvent>(this IServiceCollection services)
        where TEvent : class, IEvent
        => services.AddSingleton(_ => Mock.Of<IEventHandler<TEvent>>());
}
