using ArtistWordCount.Models;
using ArtistWordCount.Ports;
using Microsoft;
using Newtonsoft.Json.Linq;

namespace ArtistWordCount.Adapters;

public class HttpMusicMetadataAdapter : IMusicMetadata
{
    private HttpClient httpClient;

    public HttpMusicMetadataAdapter(HttpClient httpClient)
    {
        Requires.NotNull(httpClient, nameof(httpClient));
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<Artist>> GetArtistsAsync(string artistName)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"artist?query={artistName}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        string payload = await response.Content.ReadAsStringAsync();
        IEnumerable<Artist> result = ExtractArtists(payload);
        return result;
    }

    /// <summary>
    /// Extracts the artists with a 100% match from a payload.
    /// </summary>
    /// <param name="payload">The JSON payload to be parsed.</param>
    /// <returns>The list of artists.</returns>
    private IEnumerable<Artist> ExtractArtists(string payload)
    {
        JObject parsedObject = JObject.Parse(payload);
        IEnumerable<JToken> artists = parsedObject.SelectTokens("$..artists[?(@.score == 100)]");
        IEnumerable<Artist> result = artists.Select(a => new Artist(new Guid(a.SelectToken("$.id")!.Value<string>()!), a.SelectToken("$.sort-name")!.Value<string>()!));
        return result;
    }
}
