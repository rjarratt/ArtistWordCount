using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArtistWordCount.Test.Common;

public static class TestHost
{
    public static IHost CreateHost(Action<HostBuilderContext, IServiceCollection> configureServices)
    {
        IHost host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(configBuilder =>
            {
                configBuilder.AddJsonFile("testsettings.json", optional: true);
            })
            .ConfigureServices(configureServices)
            .Build();

        return host;
    }
}
