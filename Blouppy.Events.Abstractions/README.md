# Blouppy.Events.Abstractions

This project defines the core contracts for the event system.

It is completely independent of the implementation and is designed to be referenced by any layer (Domain, Application, Infrastructure, etc.).

---

## ✨ Purpose

`Blouppy.Events.Abstractions` provides shared interfaces for event-based communication between components in a clean, decoupled way.

These abstractions are used by:

- Event publishers (e.g., `Publisher`)
- Event handlers (in Application layer or elsewhere)
- The event system bootstrapper (`AddBlouppyEvents(...)` in `Blouppy.Events`)

---

## 🧱 Contents

```
Blouppy.Events.Abstractions/
├── IEvent.cs
├── IEventHandler.cs
└── IPublisher.cs
```

---

### `IEvent`

```csharp
public interface IEvent { }
```

Marker interface to represent an event.

Used to distinguish event classes from other business objects.

---

### `IEventHandler<TEvent>`

```csharp
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task Handle(TEvent evt, CancellationToken cancellationToken);
}
```

Contract for creating strongly typed event handlers.

You can register multiple handlers for the same event.

---

### `IPublisher`

```csharp
public interface IPublisher
{
    Task Publish<TEvent>(TEvent evt, CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
```

High-level abstraction to publish events from any part of the system (e.g., Application services, use cases, etc.).

The actual implementation is provided in `Blouppy.Events`.

---

## 🔒 Design Principles

- No dependencies on frameworks or infrastructure
- Pure C# abstractions

---
