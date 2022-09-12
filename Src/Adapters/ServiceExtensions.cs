using ArtistWordCount.Ports;
using Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ArtistWordCount.Adapters;

/// <summary>
/// Extensions to add adapters to the service collection.
/// </summary>
public static class ServiceExtensions
{
    private const string ProductName = "ArtistWordCount";
    private const string ProductVersion = "0.1";

    /// <summary>
    /// Sets up dependency injection for the Http implementation of the <see cref="IMusicMetadata"/> port.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration data.</param>
    /// <returns>The service collection for additional configuration.</returns>
    public static IServiceCollection AddHttpMusicMetadataAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        Requires.NotNull(configuration, nameof(configuration));

        services.AddOptions<MusicMetadataConnectionOptions>()
            .Bind(configuration.GetSection(MusicMetadataConnectionOptions.ConfigurationSectionName))
            .ValidateDataAnnotations();

        IServiceProvider configServices = services.BuildServiceProvider();
        MusicMetadataConnectionOptions musicMetadataConnectionOptions = configServices.GetRequiredService<IOptions<MusicMetadataConnectionOptions>>().Value;

        services
            .AddHttpClient<IMusicMetadata, HttpMusicMetadataAdapter>(httpClient =>
            {
                httpClient.BaseAddress = musicMetadataConnectionOptions.RootUri;
                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(ProductName, ProductVersion));
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            ////.AddTransientHttpErrorPolicy(builder => builder.Rate);

        //// TODO: Polly rate limit and retry policy

        return services;
    }
}