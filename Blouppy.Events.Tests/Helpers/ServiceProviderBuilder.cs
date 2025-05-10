using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events.Tests.Helpers;

public static class ServiceProviderBuilder
{
    public static ServiceProvider Build(
        Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        configure(services);

        return services.BuildServiceProvider();
    }
}
