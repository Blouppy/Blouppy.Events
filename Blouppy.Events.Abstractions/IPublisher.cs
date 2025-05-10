namespace Blouppy.Events.Abstractions;

/// <summary>
/// Publish an event to be handled by multiple handlers.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Asynchronously send an event to multiple handlers
    /// </summary>
    /// <param name="event">Event object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync(object @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously send an event to multiple handlers
    /// </summary>
    /// <param name="event">Event object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
