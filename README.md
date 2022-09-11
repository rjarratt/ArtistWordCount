# Artist Word Count

This is an exercise to count the words in the songs of an artist and compute statistics about the artist's songs.

# Building the Solution

To build the solution:

1. Install [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030&passive=false).
1. Install the "SpecFlow for Visual Studio 2022" extension (from the Visual Studio Extensions menu).

# Running the Solution

# Solution Design and Testing Strategy

The solution uses a [hexagonal architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software))
in order to isolate the business logic from the APIs.

This means that a comprehensive testing strategy
can be adopted that allows for most of the testing to be performed in memory and thus running very fast,
with slower integration testing confined to the testing of the API adapters only. This has the benefit
of ensuring that the developer can change the code with maximum confidence.

A Behaviour Driven Development (BDD) approach has been taken using [SpecFlow](https://docs.specflow.org/projects/specflow/en/latest/).
This means that all the tests are written in [Gherkin](https://docs.specflow.org/projects/specflow/en/latest/Gherkin/Gherkin-Reference.html).

However, in this case, owing to the need to explore and understand the APIs
the tests have not always preceded the code.

The business logic is all tested in memory using emulated adapters for each port.
Separate tests are then performed in the emulated and real adapters to prove that all implementations
of a particular port fulfil the same contract.

# Solution Structure

The solution uses `Directory.Build.props` files to apply settings across all the projects.
There is one `Directory.Build.props` in the root directory to apply settings that are common to
the entire project, such as treating all warnings as errors. The code is then divided into two directories so that different settings can apply.
These directories are:
- `src` contains all the production code
- `test` contains all the non-production code. The code here is treated as production quality but is never deployed to production.

# Resilience

The APIs used require requests to be rate limited. Furthermore, specific calls could fail with
transient errors. Therefore a rate limiting policy and retry policy is required. This is done
using the [Polly](http://www.thepollyproject.org/) package and by setting up dependency injection to create the necessary policies
so that the adapters don't need to be aware of this.
