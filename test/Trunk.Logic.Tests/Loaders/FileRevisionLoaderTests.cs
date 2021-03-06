using System.Threading.Tasks;
using FluentAssertions;
using Trunk.Logic.Loaders;
using Trunk.Logic.Parsers;
using Xunit;

namespace Trunk.Logic.Tests.Loaders;

public class FileRevisionLoaderTests
{
    [Fact]
    public async Task Should_load_file()
    {
        var loader = new FileSourceControlLogLoader(@"./Resources/code_maat.log");
        using var streamReader = await loader.LoadAsync();
        var revisionParser = new GitRevisionParser();
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        
        revisions.Should().NotBeNull();
        revisions.Should().HaveCount(315);
    }
}