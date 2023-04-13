using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Dimensions.Frequencies;
using Trunk.Logic.Tests.Fixtures;
using Xunit;

namespace Trunk.Logic.Tests.Dimensions.Frequencies;

public class AuthorsCodeLinesAddedMeasurementTests
{
    [Fact]
    public async Task Should_calculate_authors_code_added()
    {
        var revisions = await RevisionFixture.LoadSampleRevisions();

        var authorLinesAdded = AuthorsCodeLinesAddedMeasurement.Measure(revisions);

        authorLinesAdded.Should().NotBeNull();

        var codeAdded = authorLinesAdded.First(a => a.Author == "Tomasz Janiszewski");
        AssertAuthor(codeAdded, "Tomasz Janiszewski", "src/code_maat/analysis", 8);
        AssertAuthor(codeAdded, "Tomasz Janiszewski", "test/code_maat/analysis", 11);
        AssertAuthor(codeAdded, "Tomasz Janiszewski", "test/code_maat/dataset", 1);
        AssertAuthor(codeAdded, "Tomasz Janiszewski", "test/code_maat/end_to_end", 7);
    }

    private static void AssertAuthor(AuthorsCodeLinesAdded authorsCodeLinesAdded, string author, string path,
        int linesAdded)
    {
        authorsCodeLinesAdded.Should().NotBeNull();
        authorsCodeLinesAdded.Author.Should().Be(author);
        var dir = authorsCodeLinesAdded.CodeAddedToDirs.First(d => d.Path == path);
        dir.Should().NotBeNull();
        dir.NumberOfLinesAdded.Should().Be(linesAdded);
    }
}