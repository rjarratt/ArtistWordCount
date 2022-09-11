using Microsoft;

namespace ArtistWordCount.Models;

/// <summary>
/// Information about an artist.
/// </summary>
public class Artist
{
    public Artist(Guid mbId, string name)
    {
        Requires.NotEmpty(mbId, nameof(mbId));
        Requires.NotNullOrWhiteSpace(name, nameof(name));

        this.MbId = mbId;
        this.Name = name;
    }
    /// <summary>
    /// Gets or sets the Music Brainz Id of the artist
    /// </summary>
    public Guid MbId { get; private set; }

    /// <summary>
    /// Gets or sets the name of the artist.
    /// </summary>
    public string Name { get; private set; }
}
