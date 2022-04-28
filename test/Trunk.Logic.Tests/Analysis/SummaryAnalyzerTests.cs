using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Trunk.Logic.Tests.Fixtures;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class SummaryAnalyzerTests
{
    [Fact]
    public async Task Should_calculate_summary_statistics_based_on_revisions()
    {
        var revisions = await RevisionFixture.LoadSampleRevisions();

        var revisionSummary = SummaryAnalyzer.Analyze(revisions);

        revisionSummary.Should().NotBeNull();

        revisionSummary.NumberOfCommits.Should().Be(315);
        revisionSummary.NumberOfEntities.Should().Be(104);
        revisionSummary.NumberOfEntitiesChanged.Should().Be(766);
        revisionSummary.NumberOfAuthors.Should().Be(22);
    }
}