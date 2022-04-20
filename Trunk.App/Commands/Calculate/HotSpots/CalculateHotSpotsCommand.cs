using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.Logic.Analysis;
using Trunk.Logic.Loaders;
using Trunk.Logic.Parsers;

/// <summary>
/// Settings required for calculation
/// </summary>
public class CalculateHotSpotsSettings : CalculateSettings
{
    /// <summary>
    /// Path to the repository required for file lines calculation:
    /// </summary>
    [CommandOption("-r|--repo <REPO>")]
    public string RepositoryPath { get; set; }
}
/// <summary>
/// Command that calculates hot spots.
/// Configuration options: <see cref="CalculateHotSpotsSettings"/>
/// </summary>
public class CalculateHotSpotsCommand : AsyncCommand<CalculateHotSpotsSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, CalculateHotSpotsSettings settings)
    {
        var loader = new FileSourceControlLogLoader(settings.GitLogFilePath);
        using var streamReader = await loader.LoadAsync();
        var revisionParser = new GitRevisionParser();
        var revisions = await revisionParser.ParseAsync(streamReader);

        var revisionFrequency = RevisionsAnalyzer.Analyze(revisions);
        var lines = LinesOfFileCounter.Count(settings.RepositoryPath ?? ".");

        var combinedComplexities = (from entityFrequencyGroup in revisionFrequency.GroupBy(rf => rf.Entity)
            where lines.SingleOrDefault(l => l.Path == entityFrequencyGroup.Key) != null
            select new CombinedComplexity
            {
                Entity = entityFrequencyGroup.Key, NumberOfChanges = entityFrequencyGroup.Sum(g => g.RevisionNumber),
                NumberOfLines = lines.Single(l => l.Path == entityFrequencyGroup.Key).Lines
            })
            .OrderByDescending(cc => cc.NumberOfChanges)
            .ThenByDescending(cc => cc.NumberOfLines)
            .ToList();

        PrintTop10ComplexFiles(combinedComplexities.Take(10).ToList());

        return 0;
    }

    private static void PrintTop10ComplexFiles(IEnumerable<CombinedComplexity> combinedComplexities)
    {
        var table = new Table();
        table.AddColumn("Entity");
        table.AddColumn("Number of changes");
        table.AddColumn("Number of lines");

        foreach (var complexity in combinedComplexities)
        {
            table.AddRow(complexity.Entity, complexity.NumberOfChanges.ToString(), complexity.NumberOfLines.ToString());
        }

        AnsiConsole.Write(table);
    }
}

public class CombinedComplexity
{
    public string Entity { get; set; }
    
    public long NumberOfChanges { get; set; }
    public long NumberOfLines { get; set; }
}