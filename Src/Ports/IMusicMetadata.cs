using ArtistWordCount.Models;

namespace ArtistWordCount.Ports;

/// <summary>
/// Defines the operations available for obtaining music metadata.
/// </summary>
public interface IMusicMetadata
{
    /// <summary>
    /// Returns a list of artists that match the given name.
    /// </summary>
    /// <param name="artistName">The name of the artist to query for.</param>
    /// <returns>All the <see cref="Artist"/>s that have a 100% match.</returns>
    Task<IEnumerable<Artist>> GetArtistsAsync(string artistName);

    /// <summary>
    /// Returns the full list of an artist's songs.
    /// </summary>
    /// <param name="artist">The artist to get the songs for.</param>
    /// <returns>The full list of the artist's songs.</returns>
    Task<IEnumerable<Song>> GetArtistSongsAsync(Artist artist);
}