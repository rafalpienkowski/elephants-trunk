using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Analysis;
using Trunk.Logic.Tests.Fixtures;
using Xunit;

namespace Trunk.Logic.Tests.Analysis;

public class RevisionsAnalyzerTests
{
    [Fact]
    public async Task Should_calculate_entities_frequencies()
    {
        var revisions = await RevisionFixture.LoadSampleRevisions();

        var entityFrequencies = RevisionsAnalyzer.Analyze(revisions);
        
        entityFrequencies.Should().NotBeNull();

        var mostChangedEntity = entityFrequencies.First();
        AssertEntityFrequency(mostChangedEntity, "project.clj", 72);
        var secondMostChangedEntity = entityFrequencies.Skip(1).First();
        AssertEntityFrequency(secondMostChangedEntity, "src/code_maat/app/app.clj", 61);
        var thirdMostChangedEntity = entityFrequencies.Skip(2).First();
        AssertEntityFrequency(thirdMostChangedEntity, "README.md", 58);
    }

    private static void AssertEntityFrequency(EntityFrequency frequency, string entity, int numberOfRevisions)
    {
        frequency.Should().NotBeNull();
        frequency.Entity.Should().Be(entity);
        frequency.RevisionNumber.Should().Be(numberOfRevisions);
    }
}