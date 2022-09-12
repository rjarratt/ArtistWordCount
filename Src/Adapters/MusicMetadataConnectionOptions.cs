using System.ComponentModel.DataAnnotations;

namespace ArtistWordCount.Adapters;

/// <summary>
/// Defines the configuration settings for connection to the music metadata service.
/// </summary>
public class MusicMetadataConnectionOptions
{
    /// <summary>
    /// The name of the configuration section that contains the Music Metadata connection information.
    /// </summary>
    public const string ConfigurationSectionName = "MusicMetadataConnection";

    /// <summary>
    /// Gets or sets the root URI for the music metadata service.
    /// </summary>
    [Required]
    public Uri? RootUri { get; set; }
}