using System.Diagnostics;
using Blouppy.Events.EventPublishers;
using Blouppy.Events.Tests.Helpers;
using Blouppy.Events.Tests.Stubs;
using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events.Tests.EventPublishers;

public sealed class TaskWhenAllEventPublisherTests
{
    [Fact]
    public async Task Should_execute_handlers_in_parallel()
    {
        // Arrange
        var delay = TimeSpan.FromMilliseconds(200);

        var handler1 = new DelayedHandler(delay);
        var handler2 = new DelayedHandler(delay);

        var serviceProvider = ServiceProviderBuilder.Build(x => x
            .AddEventPublisher<TaskWhenAllEventPublisher>()
            .AddHandler(handler1)
            .AddHandler(handler2)
        );
        
        var sut = BuildSut(serviceProvider);

        var evt = new Event1();
        
        // Act
        var sw = Stopwatch.StartNew();

        await sut.PublishAsync(evt);

        sw.Stop();
        
        // Assert
        sw.Elapsed
            .Should()
            .BeLessThan(delay * 2);
    }
    
    private static Publisher BuildSut(IServiceProvider provider) 
        => new(provider, provider.GetRequiredService<IEventPublisher>());
}
