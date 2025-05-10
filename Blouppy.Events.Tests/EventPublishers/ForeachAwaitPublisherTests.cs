using System.Diagnostics;
using Blouppy.Events.EventPublishers;
using Blouppy.Events.Tests.Helpers;
using Blouppy.Events.Tests.Stubs;
using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events.Tests.EventPublishers;

public sealed class ForeachAwaitPublisherTests
{
    [Fact]
    public async Task Should_execute_handlers_sequentially()
    {
        // Arrange
        var delay = TimeSpan.FromMilliseconds(200);

        var handler1 = new DelayedHandler(delay);
        var handler2 = new DelayedHandler(delay);

        var serviceProvider = ServiceProviderBuilder.Build(x => x
            .AddEventPublisher<ForeachAwaitPublisher>()
            .AddHandler(handler1)
            .AddHandler(handler2)
        );
        
        var sut = BuildSut(serviceProvider);

        var @event = new Event1();
        
        // Act
        var sw = Stopwatch.StartNew();

        await sut.PublishAsync(@event);

        sw.Stop();
        
        // Assert
        sw.Elapsed
            .Should()
            .BeGreaterThanOrEqualTo(delay * 2);
    }
    
    private static Publisher BuildSut(IServiceProvider provider) 
        => new(provider, provider.GetRequiredService<IEventPublisher>());
}
