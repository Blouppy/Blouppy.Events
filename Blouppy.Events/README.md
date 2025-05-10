# Blouppy.Events

**Blouppy.Events** is an event dispatching system.

It enables clean and decoupled event publishing with support for multiple handlers, scoped DI registration, and configurable dispatching strategies — all without external dependencies.

---

## 📦 Projects

### 🔹 `Blouppy.Events.Abstractions`

Defines the core contracts:

- `IEvent` — marker interface for events
- `IEventHandler<TEvent>` — generic handler interface
- `IPublisher` — public interface for publishing events

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

- `Publisher.cs`: main implementation of `IPublisher`
- `ServiceCollectionExtensions.cs`: exposes `AddBlouppyEvents(...)` for DI setup
- `IEventPublisher`: internal interface to define execution strategy
- `ForeachAwaitPublisher`: sequential strategy (default)
- `TaskWhenAllPublisher`: parallel strategy

---

## 🚀 Quick Start

### 1. Define an event

```csharp
public class UserCreatedEvent : IEvent
{
    public Guid UserId { get; set; }

    public UserCreatedEvent(Guid userId) => UserId = userId;
}
```

### 2. Create a handler

```csharp
public class SendWelcomeEmailHandler : IEventHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent evt, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Welcome email sent to user {evt.UserId}");
        return Task.CompletedTask;
    }
}
```

### 3. Register the system

In `Program.cs` or your composition root:

```csharp
services.AddBlouppyEvents(
    typeof(SendWelcomeEmailHandler).Assembly
);
```

This will:

- Register `IPublisher` and `IEventPublisher`
- Discover and register all `IEventHandler<T>` implementations from the given assemblies

---

## ⚙️ Dispatching Strategies

Choose your event dispatch strategy:

```csharp
services.AddScoped<IEventPublisher, ForeachAwaitPublisher>(); // sequential (default)
services.AddScoped<IEventPublisher, TaskWhenAllPublisher>();  // parallel
```

---

## 🧠 Notes

- `Publisher` resolves all matching `IEventHandler<T>` via DI
- `AddBlouppyEvents` registers handlers from the given assemblies

---