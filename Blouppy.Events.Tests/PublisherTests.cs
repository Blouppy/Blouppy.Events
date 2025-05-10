using Blouppy.Events.Abstractions;
using Blouppy.Events.EventPublishers;
using Blouppy.Events.Tests.Helpers;
using Blouppy.Events.Tests.Stubs;
using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events.Tests;

public sealed class PublisherTests
{
    [Fact]
    public async Task Should_not_throw_for_one_handler()
    {
        // Arrange
        var serviceProvider = ServiceProviderBuilder.Build(x => x
            .AddEventPublisher<ForeachAwaitPublisher>()
            .AddMockedEventHandler<Event1>()
        );

        var sut = BuildSut(serviceProvider);
        
        var @event = new Event1();

        // Act
        var act = sut.Invoking(x => x.PublishAsync(@event));

        // Assert
        await act
            .Should()
            .NotThrowAsync();
    }
    
    [Fact]
    public async Task Should_not_throw_for_multiple_handlers()
    {
        // Arrange
        var serviceProvider = ServiceProviderBuilder.Build(x => x
            .AddEventPublisher<ForeachAwaitPublisher>()
            .AddMockedEventHandler<Event1>()
            .AddMockedEventHandler<Event1>()
        );

        var eventPublisher = serviceProvider.GetRequiredService<IEventPublisher>();
        
        var sut = new Publisher(serviceProvider, eventPublisher);
        
        var @event = new Event1();

        // Act
        var act = sut.Invoking(x => x.PublishAsync(@event));

        // Assert
        await act
            .Should()
            .NotThrowAsync();
    }
    
    [Fact]
    public async Task Should_not_throw_when_no_handler()
    {
        // Arrange
        var serviceProvider = ServiceProviderBuilder.Build(x => x
            .AddEventPublisher<ForeachAwaitPublisher>()
        );
        
        var sut = BuildSut(serviceProvider);
        
        var @event = new Event1();

        // Act
        var act = sut.Invoking(x => x.PublishAsync(@event));

        // Assert
        await act
            .Should()
            .NotThrowAsync();
    }
    
    [Fact]
    public async Task Should_handle_appropriate_handlers()
    {
        var handler1 = Mock.Of<IEventHandler<Event1>>();
        var handler1Bis = Mock.Of<IEventHandler<Event1>>();
        var handler2 = Mock.Of<IEventHandler<Event2>>();

        // Arrange
        var serviceProvider = ServiceProviderBuilder.Build(x => x
            .AddEventPublisher<ForeachAwaitPublisher>()
            .AddHandler(handler1)
            .AddHandler(handler1Bis)
            .AddHandler(handler2)
        );
        
        var sut = BuildSut(serviceProvider);
        
        var @event = new Event1();

        // Act
        await sut.PublishAsync(@event);

        // Assert
        Mock.Get(handler1)
            .Verify(x => x.HandleAsync(@event, It.IsAny<CancellationToken>()), Times.Once);

        Mock.Get(handler1Bis)
            .Verify(x => x.HandleAsync(@event, It.IsAny<CancellationToken>()), Times.Once);

        Mock.Get(handler2)
            .Verify(x => x.HandleAsync(It.IsAny<Event2>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    private static Publisher BuildSut(IServiceProvider provider) 
        => new(provider, provider.GetRequiredService<IEventPublisher>());
}
