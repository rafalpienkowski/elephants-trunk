using System.Collections.Generic;
using System.Threading.Tasks;
using Trunk.Logic.Loaders;
using Trunk.Logic.Models;
using Trunk.Logic.Parsers;

namespace Trunk.Logic.Tests.Fixtures;

/// <summary>
/// Loads and parses code_maat.log from resources and use revisions to perform further operations
/// </summary>
public static class RevisionFixture
{
    public static async Task<IList<Revision>> LoadSampleRevisions()
    {
        var loader = new FileSourceControlLogLoader(@"./Resources/code_maat.log");
        using var streamReader = await loader.LoadAsync();
        var revisionParser = new GitRevisionParser();
        return await revisionParser.ParseAsync(streamReader);
    }
}