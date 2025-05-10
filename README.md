# Blouppy.Events

**Blouppy.Events** is a lightweight and flexible in memory event publisher for .NET.

## 📦 Solution Structure

The solution consists of two projects:

### 🔹 `Blouppy.Events.Abstractions`

Defines the core contracts:
- `IEvent` — Marker interface for events
- `IEventHandler<TEvent>` — Generic handler interface
- `IPublisher` — Public interface for publishing events

### 🔹 `Blouppy.Events`

Implements the event system:
```
Blouppy.Events/
├── EventHandlers/
│   ├── EventHandlerExecutor.cs
│   └── EventHandlerWrapper.cs
│
├── EventPublishers/
│   ├── IEventPublisher.cs
│   ├── ForeachAwaitPublisher.cs
│   └── TaskWhenAllPublisher.cs
│
├── Publisher.cs
└── ServiceCollectionExtensions.cs
```

## 🚀 Quick Start

### 1. Define an event

```csharp
public sealed record MyEvent : IEvent;
```

### 2. Create a handler

```csharp
public sealed class MyEventHandler(
    TimeSpan delay) : IEventHandler<MyEvent>
{
    public async Task HandleAsync(MyEvent @event, CancellationToken cancellationToken) 
        => await Task.Delay(delay, cancellationToken);
}
```

### 3. Register the system

In `Program.cs` or your composition root:

```csharp
services.AddBlouppyEvents(options =>
{
    options.RegisterServicesFromAssemblies(typeof(MyEventHandler).Assembly);
    
    // Optional: Configure the event publisher strategy
    options.EventPublisherType = typeof(TaskWhenAllEventPublisher);
    
    // Optional: Configure the service lifetime (default is Scoped)
    options.Lifetime = ServiceLifetime.Scoped;
});
```

## ⚙️ Dispatching Strategies

Choose your event dispatching strategy (or implement your own):

```csharp
// Sequential (default)
options.EventPublisherType = typeof(ForeachAwaitEventPublisher);

// Parallel
options.EventPublisherType = typeof(TaskWhenAllEventPublisher);
```

## 🧠 Notes

- The `Publisher` resolves all matching `IEventHandler<T>` via dependency injection
- `AddBlouppyEvents` registers handlers from the specified assemblies
- Full dependency injection support with scoping
- No external dependencies required

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details. 