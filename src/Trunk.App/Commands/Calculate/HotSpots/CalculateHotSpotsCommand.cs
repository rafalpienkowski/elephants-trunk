using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.Logic.Analysis;
using Trunk.Logic.Dimensions.Complexities;
using Trunk.Logic.Dimensions.Frequencies;
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
        await AnsiConsole.Status().StartAsync("Thinking...", async ctx =>
        {
            ctx.Spinner(Spinner.Known.Star);
            ctx.SpinnerStyle(Style.Parse("green"));
            
            ctx.Status("Loading git log file");
            var loader = new FileSourceControlLogLoader(settings.GitLogFilePath);
            using var streamReader = await loader.LoadAsync();
            ctx.Status("Parsing git revisions");
            var revisionParser = new GitRevisionParser();
            var revisions = await GitRevisionParser.ParseAsync(streamReader);

            ctx.Status("Calculating changes");
            var revisionFrequency = ChangesInFileMeasurement.Measure(revisions);
            
            ctx.Status("Counting file lines");
            var lines = LinesOfCodeMeasurement.Measure(settings.RepositoryPath ?? ".");

            ctx.Status("Calculating combined complexity");
            var combinedComplexities = CalculateCombinedComplexities(revisionFrequency, lines);

            ctx.Status("Saving results");
            await SaveResultsToCsvAsync(combinedComplexities);
            
            PrintTop10ComplexFiles(combinedComplexities.Take(10).ToList());
        });

        return 0;
    }

    private static List<CombinedComplexity> CalculateCombinedComplexities(List<FrequencyOfChanges> revisionFrequency, List<CodeLines> lines)
    {
        var combinedComplexities = (from entityFrequencyGroup in revisionFrequency.GroupBy(rf => rf.Path)
                where lines.SingleOrDefault(l => l.Path == entityFrequencyGroup.Key) != null
                select new CombinedComplexity
                {
                    Entity = entityFrequencyGroup.Key,
                    NumberOfChanges = entityFrequencyGroup.Sum(g => g.NumberOfChanges),
                    NumberOfLines = lines.Single(l => l.Path == entityFrequencyGroup.Key).Lines
                })
            .OrderByDescending(cc => cc.NumberOfChanges)
            .ThenByDescending(cc => cc.NumberOfLines)
            .ToList();
        return combinedComplexities;
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

    private static async Task SaveResultsToCsvAsync(IEnumerable<CombinedComplexity> combinedComplexities)
    {
        const string resultFileName = "hotspots.csv";
        if (File.Exists(resultFileName))
        {
            File.Delete(resultFileName);
        }

        await using var sw = new StreamWriter(resultFileName);
        await sw.WriteLineAsync("Entity,Number of changes,Number of lines");
        foreach (var combinedComplexity in combinedComplexities)
        {
            await sw.WriteLineAsync($"{combinedComplexity.Entity},{combinedComplexity.NumberOfChanges},{combinedComplexity.NumberOfLines}");
        }
    }
}

public class CombinedComplexity
{
    public string Entity { get; set; }
    public long NumberOfChanges { get; set; }
    public long NumberOfLines { get; set; }
}