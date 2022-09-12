using ArtistWordCount.Models;
using ArtistWordCount.Ports;
using Microsoft;
using Newtonsoft.Json.Linq;

namespace ArtistWordCount.Adapters;

public class HttpMusicMetadataAdapter : IMusicMetadata
{
    private HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMusicMetadataAdapter"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> to be used for calling the Music Metadata service.</param>
    public HttpMusicMetadataAdapter(HttpClient httpClient)
    {
        Requires.NotNull(httpClient, nameof(httpClient));
        this.httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Artist>> GetArtistsAsync(string artistName)
    {
        HttpResponseMessage response = await this.httpClient.GetAsync(new Uri($"artist?query={artistName}", UriKind.Relative)).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        IEnumerable<Artist> result = ExtractArtists(payload);
        return result;
    }

    /// <summary>
    /// Extracts the artists with a 100% match from a payload.
    /// </summary>
    /// <param name="payload">The JSON payload to be parsed.</param>
    /// <returns>The list of artists.</returns>
    private static IEnumerable<Artist> ExtractArtists(string payload)
    {
        JObject parsedObject = JObject.Parse(payload);
        IEnumerable<JToken> artists = parsedObject.SelectTokens("$..artists[?(@.score == 100)]");
        IEnumerable<Artist> result = artists.Select(a => new Artist(new Guid(a.SelectToken("$.id")!.Value<string>()!), a.SelectToken("$.sort-name")!.Value<string>()!));
        return result;
    }
}