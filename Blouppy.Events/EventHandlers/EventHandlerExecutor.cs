using Blouppy.Events.Abstractions;

namespace Blouppy.Events.EventHandlers;

internal record EventHandlerExecutor(
    object HandlerInstance, 
    Func<IEvent, CancellationToken, Task> HandlerCallback);
