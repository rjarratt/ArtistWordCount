using ArtistWordCount.Models;
using ArtistWordCount.Ports;
using Microsoft;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ArtistWordCount.Adapters;

/// <summary>
/// The MusicBrainz adapter for the <see cref="IMusicMetadata"/> port.
/// </summary>
/// <remarks>
/// The API reference is here: https://musicbrainz.org/doc/MusicBrainz_API.
/// </remarks>
public class HttpMusicMetadataAdapter : IMusicMetadata
{
    private const int MinimumScore = 90; // the minimum score to consider an artist a match.
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
        // Note that we do not do pagination here. The API only seems to ever return a single match with a 100% score. We set the bar
        // a bit lower than 100% to return close matches, but even with a search term such as "the", we only get a handful scoring above
        // 90% so there is no point paginating here. In any case, presenting a user with a very long list of matches is not going to be
        // very helpful anyway.

        // TODO: escape Lucene special characters: https://lucene.apache.org/core/4_3_0/queryparser/org/apache/lucene/queryparser/classic/package-summary.html#Escaping_Special_Characters.
        // Don't currently do this because it isn't entirely clear how the inclusion of the special characters actually affects the query, it seems to work
        // without escaping.
        HttpResponseMessage response = await this.httpClient.GetAsync(new Uri($"artist?query={WebUtility.UrlEncode(artistName)}", UriKind.Relative)).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        IEnumerable<Artist> result = ExtractArtists(payload);
        return result;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Song>> GetArtistSongsAsync(Artist artist)
    {
        Requires.NotNull(artist, nameof(artist));
        List<Song> result = new List<Song>();
        List<Song> pageOfSongs;
        int offset = 0;

        do
        {
            pageOfSongs = (await this.GetPageOfSongs(artist, offset).ConfigureAwait(false)).ToList();
            result.AddRange(pageOfSongs);
            offset += pageOfSongs.Count;
        }
        while (pageOfSongs.Any());

        return result;
    }

    /// <summary>
    /// Extracts the artists with a <see cref="MinimumScore"/> match from a payload.
    /// </summary>
    /// <param name="payload">The JSON payload to be parsed.</param>
    /// <returns>The list of artists.</returns>
    private static IEnumerable<Artist> ExtractArtists(string payload)
    {
        JObject parsedObject = JObject.Parse(payload);
        IEnumerable<JToken> artists = parsedObject.SelectTokens($"$..artists[?(@.score >= {MinimumScore})]");
        IEnumerable<Artist> result = artists.Select(a => new Artist(new Guid(a.SelectToken("$.id")!.Value<string>()!), a.SelectToken("$.sort-name")!.Value<string>()!));
        return result;
    }

    private static IEnumerable<Song> ExtractSongs(string payload)
    {
        JObject parsedObject = JObject.Parse(payload);
        IEnumerable<JToken> songs = parsedObject.SelectTokens($"$..works[?(@.type >= 'Song')]");
        IEnumerable<Song> result = songs.Select(a => new Song(new Guid(a.SelectToken("$.id")!.Value<string>()!), a.SelectToken("$.title")!.Value<string>()!));
        return result;
    }

    private async Task<IEnumerable<Song>> GetPageOfSongs(Artist artist, int offset)
    {
        HttpResponseMessage response = await this.httpClient.GetAsync(new Uri($"work?artist={artist.MbId}&offset={offset}&limit=100", UriKind.Relative)).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        IEnumerable<Song> result = ExtractSongs(payload);
        return result;
    }
}