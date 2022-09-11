using System.ComponentModel.DataAnnotations;

namespace ArtistWordCount.Adapters;

/// <summary>
/// Defines the configuration settings for connection to the music metadata service.
/// </summary>
public class MusicMetadataConnectionOptions
{
    public const string ConfigurationSectionName = "MusicMetadataConnection";

    /// <summary>
    /// Gets or sets the root URI for the music metadata service.
    /// </summary>
    [Required]
    public Uri? RootUri { get; set; }
}
