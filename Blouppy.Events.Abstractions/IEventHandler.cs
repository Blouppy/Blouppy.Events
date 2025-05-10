namespace Blouppy.Events.Abstractions;

/// <summary>
/// Defines a handler for an event
/// </summary>
/// <typeparam name="TEvent">The type of event being handled</typeparam>
public interface IEventHandler<in TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Handles an event
    /// </summary>
    /// <param name="event">The event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
