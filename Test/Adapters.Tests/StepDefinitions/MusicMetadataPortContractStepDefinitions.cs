using ArtistWordCount.Models;
using ArtistWordCount.Ports;
using ArtistWordCount.Test.Common;
using FluentAssertions;
using Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ArtistWordCount.Adapters.Tests.StepDefinitions;

[Binding]
public class MusicMetadataPortContractStepDefinitions : IDisposable
{
    private IHost? host;
    private IMusicMetadata? musicMetadataPort;
    private Exception? caughtException;
    private List<Artist>? artistList;
    private List<Song>? songList;
    private bool disposedValue;

    [StepArgumentTransformation]
    public static Artist TransformArtist(string artistString)
    {
        Requires.NotNullOrWhiteSpace(artistString, nameof(artistString));

        string[] parts = artistString.Split(':');
        Artist result = new Artist(Guid.Parse(parts[1]), parts[0]);
        return result;
    }

    [StepArgumentTransformation]
    public static IEnumerable<Artist> TransformListOfArtists(Table table)
    {
        IEnumerable<Artist> artists = table.CreateSet<Artist>((row) => new Artist(Guid.NewGuid(), row[0]));
        return artists;
    }

    [StepArgumentTransformation]
    public static IEnumerable<Song> TransformListOfSongs(Table table)
    {
        IEnumerable<Song> songs = table.CreateSet<Song>((row) => new Song(Guid.Parse(row[1]), row[0]));
        return songs;
    }

    [Given(@"I am using the (.*) music metadata adapter")]
    public void GivenIAmUsingTheMusicMetadataAdapter(string adapterType)
    {
        this.host = TestHostingExtensions.CreateHost((hostBuilderContext, services) =>
        {
            if (adapterType == "Http")
            {
                services.AddHttpMusicMetadataAdapter(hostBuilderContext.Configuration!);
            }
            else
            {
                throw new NotImplementedException();
            }
        });

        this.musicMetadataPort = this.host.Services.GetRequiredService<IMusicMetadata>();
    }

    [When(@"I query for the artist '([^']*)'")]
    public async Task WhenIQueryForTheArtist(string artistName)
    {
        try
        {
            this.artistList = (await this.musicMetadataPort!.GetArtistsAsync(artistName).ConfigureAwait(false)).ToList();
        }
        catch (HttpRequestException hre)
        {
            this.caughtException = hre;
        }
    }

    [When(@"I query for the songs of '([^']*)'")]
    public async Task WhenIQueryForTheSongsOf(Artist artist)
    {
        try
        {
            this.songList = (await this.musicMetadataPort!.GetArtistSongsAsync(artist).ConfigureAwait(false)).ToList();
        }
        catch (HttpRequestException hre)
        {
            this.caughtException = hre;
        }
    }

    [When(@"I query for '([^']*)' (.*) times in a row")]
    public async Task WhenIQueryForTimesInARow(string artistName, int numberOfRepeats)
    {
        for (int i = 0; i < numberOfRepeats; i++)
        {
            await this.WhenIQueryForTheArtist(artistName).ConfigureAwait(false);
        }
    }

    [Then(@"no error occurs")]
    public void ThenNoErrorOccurs()
    {
        this.caughtException.Should().BeNull();
    }

    [Then(@"I get the following list of artists:")]
    public void ThenIGetTheFollowingListOfArtists(IEnumerable<Artist> expectedArtists)
    {
        this.artistList.Should().BeEquivalentTo(expectedArtists);
    }

    [Then(@"I get no results")]
    public void ThenIGetNoResults()
    {
        this.artistList.Should().BeEmpty();
    }

    [Then(@"I get a list of (.*) songs including:")]
    public void ThenIGetAListOfSongsIncluding(int numberOfSongs, IEnumerable<Song> someExpectedSongs)
    {
        Requires.NotNull(someExpectedSongs, nameof(someExpectedSongs));

        this.songList.Should().HaveCount(numberOfSongs);
        foreach (Song expectedSong in someExpectedSongs)
        {
            this.songList.Should().ContainEquivalentOf(expectedSong);
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.host?.Dispose();
            }

            this.disposedValue = true;
        }
    }
}