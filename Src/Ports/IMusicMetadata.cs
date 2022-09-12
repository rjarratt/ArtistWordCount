using ArtistWordCount.Models;

namespace ArtistWordCount.Ports;

/// <summary>
/// Defines the operations available for obtaining music metadata
/// </summary>
public interface IMusicMetadata
{
    /// <summary>
    /// Returns a list of artists that match the given name.
    /// </summary>
    /// <param name="artistName">The name of the artist to query for.</param>
    /// <returns>All the <see cref="Artist"/>s that have a 100% match.</returns>
    Task<IEnumerable<Artist>> GetArtistsAsync(string artistName);
}