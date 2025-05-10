using Blouppy.Events.Abstractions;

namespace Blouppy.Events.EventHandlers;

/// <summary>
/// Represents an event handler executor.
/// </summary>
/// <param name="HandlerInstance">The instance of the handler.</param>
/// <param name="HandlerCallback">The callback to invoke the handler.</param>
public record EventHandlerExecutor(
    object HandlerInstance, 
    Func<IEvent, CancellationToken, Task> HandlerCallback);
