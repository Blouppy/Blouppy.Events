using Blouppy.Events.Abstractions;

namespace Blouppy.Events.Tests.Stubs;

// Fake event handler for testing
public sealed class DelayedHandler(
    TimeSpan delay) : IEventHandler<Event1>
{
    public async Task HandleAsync(Event1 @event, CancellationToken cancellationToken) 
        => await Task.Delay(delay, cancellationToken);
}
