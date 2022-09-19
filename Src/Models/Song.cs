using Microsoft;

namespace ArtistWordCount.Models;

/// <summary>
/// Information about a song.
/// </summary>
public class Song
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Song"/> class.
    /// </summary>
    /// <param name="mbId">The MusicBrainz Id of the song.</param>
    /// <param name="title">The title of the song.</param>
    public Song(Guid mbId, string title)
    {
        Requires.NotEmpty(mbId, nameof(mbId));
        Requires.NotNullOrWhiteSpace(title, nameof(title));

        this.MbId = mbId;
        this.Title = title;
    }

    /// <summary>
    /// Gets the Music Brainz Id of the artist.
    /// </summary>
    public Guid MbId { get; private set; }

    /// <summary>
    /// Gets the title of the song.
    /// </summary>
    public string Title { get; private set; }
}
