using Microsoft;

namespace ArtistWordCount.Models;

/// <summary>
/// Information about an artist.
/// </summary>
public class Artist
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Artist"/> class.
    /// </summary>
    /// <param name="mbId">The MusicBrainz Id of the artist.</param>
    /// <param name="name">The name of the artist.</param>
    public Artist(Guid mbId, string name)
    {
        Requires.NotEmpty(mbId, nameof(mbId));
        Requires.NotNullOrWhiteSpace(name, nameof(name));

        this.MbId = mbId;
        this.Name = name;
    }

    /// <summary>
    /// Gets the Music Brainz Id of the artist.
    /// </summary>
    public Guid MbId { get; private set; }

    /// <summary>
    /// Gets the name of the artist.
    /// </summary>
    public string Name { get; private set; }
}