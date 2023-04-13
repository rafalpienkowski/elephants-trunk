using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Parsers;
using Xunit;

namespace Trunk.Logic.Tests.Parsers;

public class GitRevisionParserTests
{
    private const string SingleRevision = @"[c41a1ae] Adam Tornhill 2020-12-29 Remove dead code such as earlier prototypes
1	14	src/code_maat/analysis/authors.clj
0	47	src/code_maat/parsers/limitters.clj";

    private const string MultipleRevisions = @"[1e0a9a9] Adam Tornhill 2020-12-29 #57 The messages analysis is not supported for git2
22	4	src/code_maat/analysis/commit_messages.clj
2	0	src/code_maat/app/app.clj
24	1	test/code_maat/analysis/commit_messages_test.clj
10	1	test/code_maat/end_to_end/scenario_tests.clj

[7b5d0b6] Александар Симић 2020-12-25 Updated the README
5	5	README.md

[94f5a8b] Александар Симић 2020-12-24 Whitespace cleanup
0	1	src/code_maat/analysis/code_age.clj
1	1	src/code_maat/analysis/commit_messages.clj
2	3	src/code_maat/analysis/communication.clj
2	2	src/code_maat/analysis/coupling_algos.clj
4	4	src/code_maat/analysis/sum_of_coupling.clj
1	1	src/code_maat/analysis/summary.clj
2	2	src/code_maat/parsers/git.clj
1	1	src/code_maat/parsers/limitters.clj
5	5	src/code_maat/parsers/tfs.clj
1	1	src/code_maat/test/data_driven.clj";

    private const string FileRename =
        @"[cfe0bf575] Oskar Dudycz 2022-08-05 Defined first version of public API for Upcasters
8	0	src/Marten/Events/EventGraph.cs
33	0	src/Marten/Services/Json/Transformations/EventUpcaster.cs
30	37	src/Marten/Services/Json/{JsonTransformationSerializerWrapper.cs => Transformations/JsonTransformations.cs}
65	0	src/Marten/Services/Json/Transformations/SystemTextJson/SystemTextJsonUpcasters.cs
26	1	src/Marten/Services/SystemTextJsonSerializer.cs
6	3	src/Marten/StoreOptions.cs
5	0	src/Marten/Util/SharedBuffer.cs";
    
    [Fact]
    public async Task Should_parse_single_revision_with_multiple_file_changes()
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(SingleRevision));
        var streamReader = new StreamReader(memoryStream);
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        
        revisions.Should().NotBeNull();
        revisions.Should().HaveCount(1);
        var firstRevision = revisions.First();
        AssertRevisionWithoutFileChange(firstRevision, "Adam Tornhill", "Remove dead code such as earlier prototypes",
            new DateTime(2020, 12, 29, 0, 0, 0), 2);

        var firstFileChange = firstRevision.FileChanges.First();
        AssertFileChange(firstFileChange, 1, 14, "src/code_maat/analysis/authors.clj");
        var lastFileChange = firstRevision.FileChanges.Last();
        AssertFileChange(lastFileChange, 0, 47, "src/code_maat/parsers/limitters.clj");
    }

    [Fact]
    public async Task Should_parse_multiple_revisions_with_multiple_file_changes()
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(MultipleRevisions));
        var streamReader = new StreamReader(memoryStream);
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        
        revisions.Should().NotBeNull();
        revisions.Should().HaveCount(3);

        var firstRevision = revisions.First();
        AssertRevisionWithoutFileChange(firstRevision, "Adam Tornhill",
            "#57 The messages analysis is not supported for git2", new DateTime(2020, 12, 29, 0, 0, 0), 4);
    }

    [Fact]
    public async Task Should_parse_rename_file()
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(FileRename));
        var streamReader = new StreamReader(memoryStream);
        
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        
        revisions.Should().NotBeNull();
        revisions.Should().HaveCount(1);
        revisions[0].FileChanges[2].FilePath.Should()
            .Be("src/Marten/Services/Json/Transformations/JsonTransformations.cs");
    }

    private static void AssertRevisionWithoutFileChange(Revision revision, string author, string message,
        DateTime date, int fileChanges)
    {
        revision.Should().NotBeNull();
        revision.Author.Should().Be(author);
        revision.Date.Should().Be(date);
        revision.Message.Should().Be(message);
        revision.FileChanges.Should().HaveCount(fileChanges);
    }

    private static void AssertFileChange(FileChange fileChange, uint linesAdded, uint linesRemoved, string filePath)
    {
        fileChange.Should().NotBeNull();
        fileChange.LinesAdded.Should().Be(linesAdded);
        fileChange.LinesRemoved.Should().Be(linesRemoved);
        fileChange.FilePath.Should().Be(filePath);
    }
}