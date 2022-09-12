using ArtistWordCount.Models;
using ArtistWordCount.Ports;
using ArtistWordCount.Test.Common;
using FluentAssertions;
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
    private List<Artist>? artistList;
    private bool disposedValue;

    [StepArgumentTransformation]
    public static IEnumerable<Artist> TransformListOfArtists(Table table)
    {
        IEnumerable<Artist> artists = table.CreateSet<Artist>((row) => new Artist(Guid.NewGuid(), row[0]));
        return artists;
    }

    [Given(@"I am using the (.*) music metadata adapter")]
    public void GivenIAmUsingTheMusicMetadataAdapter(string adapterType)
    {
        this.host = TestHost.CreateHost((hostBuilderContext, services) =>
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

    [When(@"I query for '([^']*)'")]
    public async Task WhenIQueryFor(string artistName)
    {
        this.artistList = (await this.musicMetadataPort!.GetArtistsAsync(artistName).ConfigureAwait(false)).ToList();
    }

    [When(@"I query for '([^']*)' (.*) times in a row")]
    public async Task WhenIQueryForTimesInARow(string artistName, int numberOfRepeats)
    {
        for (int i = 0; i < numberOfRepeats; i++)
        {
            await this.WhenIQueryFor(artistName).ConfigureAwait(false);
        }
    }

    [Then(@"I get the following list of artists:")]
    public void ThenIGetTheFollowingListOfArtists(IEnumerable<Artist> expectedArtists)
    {
        this.artistList.Should().BeEquivalentTo(expectedArtists);
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