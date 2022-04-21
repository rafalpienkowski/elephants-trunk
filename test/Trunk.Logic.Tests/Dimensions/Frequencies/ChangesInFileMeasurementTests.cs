using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Dimensions.Frequencies;
using Trunk.Logic.Tests.Fixtures;
using Xunit;

namespace Trunk.Logic.Tests.Dimensions.Frequencies;

public class ChangesInFileMeasurementTests
{
    [Fact]
    public async Task Should_calculate_entities_frequencies()
    {
        var revisions = await RevisionFixture.LoadSampleRevisions();

        var entityFrequencies = ChangesInFileMeasurement.Measure(revisions);
        
        entityFrequencies.Should().NotBeNull();

        var mostChangedEntity = entityFrequencies.First();
        AssertEntityFrequency(mostChangedEntity, "project.clj", 72);
        var secondMostChangedEntity = entityFrequencies.Skip(1).First();
        AssertEntityFrequency(secondMostChangedEntity, "src/code_maat/app/app.clj", 61);
        var thirdMostChangedEntity = entityFrequencies.Skip(2).First();
        AssertEntityFrequency(thirdMostChangedEntity, "README.md", 58);
    }

    private static void AssertEntityFrequency(FrequencyOfChanges frequencyOfChanges, string entity, int numberOfRevisions)
    {
        frequencyOfChanges.Should().NotBeNull();
        frequencyOfChanges.Path.Should().Be(entity);
        frequencyOfChanges.NumberOfChanges.Should().Be(numberOfRevisions);
    }
}