using Blouppy.Events.Abstractions;

namespace Blouppy.Events.EventHandlers;

public record EventHandlerExecutor(
    object HandlerInstance, 
    Func<IEvent, CancellationToken, Task> HandlerCallback);
