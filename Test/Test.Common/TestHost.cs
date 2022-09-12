using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArtistWordCount.Test.Common;

/// <summary>
/// Provides hosting for tests.
/// </summary>
public static class TestHost
{
    /// <summary>
    /// Creates the hosting for running tests.
    /// </summary>
    /// <param name="configureServices">The action that configures the services.</param>
    /// <returns>A built host.</returns>
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