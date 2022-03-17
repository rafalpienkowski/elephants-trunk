using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Parsers;
using Xunit;

namespace Trunk.Logic.Tests;

public class GitRevisionParserTests
{
    private readonly GitRevisionParser _gitRevisionParser = new();
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
    
    [Fact]
    public async Task Should_parse_single_revision_with_multiple_file_changes()
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(SingleRevision));
        var streamReader = new StreamReader(memoryStream);
        var revisions = await _gitRevisionParser.ParseAsync(streamReader);
        
        revisions.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_parse_multiple_revisions_with_multiple_file_changes()
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(MultipleRevisions));
        var streamReader = new StreamReader(memoryStream);
        var revisions = await _gitRevisionParser.ParseAsync(streamReader);
        
        revisions.Should().NotBeNull();
    }

}