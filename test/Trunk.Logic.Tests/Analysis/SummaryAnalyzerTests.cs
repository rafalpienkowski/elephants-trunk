using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Trunk.Logic.Tests.Fixtures;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class SummaryAnalyzerTests
{
    private readonly SummaryAnalyzer _sut = new();

    [Fact]
    public async Task Should_calculate_summary_statistics_based_on_revisions()
    {
        var revisions = await RevisionFixture.LoadSampleRevisions();

        var analyzeResult = _sut.Analyze(revisions);

        analyzeResult.Should().NotBeNull();
        analyzeResult.Should().BeOfType(typeof(SummaryAnalysisResult));
        var summaryResult = (SummaryAnalysisResult) analyzeResult;

        summaryResult.NumberOfCommits.Should().Be(315);
        summaryResult.NumberOfEntities.Should().Be(104);
        summaryResult.NumberOfEntitiesChanged.Should().Be(766);
        summaryResult.NumberOfAuthors.Should().Be(22);
    }
}