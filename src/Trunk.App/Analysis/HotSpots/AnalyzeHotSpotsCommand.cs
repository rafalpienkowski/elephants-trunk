using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Complexities;
using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.App.Analysis.HotSpots;

/// <summary>
/// Command that calculates hot spots.
/// Configuration options: <see cref="AnalyzeHotSpotsSettings"/>
/// </summary>
public class AnalyzeHotSpotsCommand : AsyncCommand<AnalyzeHotSpotsSettings>
{
    private const string OutputFileName = "hotspots.csv";
    
    public override async Task<int> ExecuteAsync(CommandContext context, AnalyzeHotSpotsSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Thinking...", async ctx =>
        {
            ctx.SetupSpinner();
            
            ctx.Status("Loading lines of code file");
            var lines = settings.LinesOfCodeFilePath.ReadFromCsvFile<CodeLines>().ToList();
            
            ctx.Status("Loading code frequencies file");
            var revisionFrequency = settings.CodeFrequenciesFilePath.ReadFromCsvFile<FrequencyOfChanges>();
            
            ctx.Status("Analyzing hot spots");
            var combinedComplexities = CalculateCombinedComplexities(revisionFrequency, lines);

            ctx.Status("Saving results");
            await OutputFileName.WriteToCsvFile(combinedComplexities);
            
            PrintTop10ComplexFiles(combinedComplexities.Take(10).ToList());
        });

        return 0;
    }
    
    private static List<CombinedComplexity> CalculateCombinedComplexities(IEnumerable<FrequencyOfChanges> revisionFrequency, IReadOnlyCollection<CodeLines> lines)
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

    private class CombinedComplexity
    {
        public string Entity { get; set; }
        public long NumberOfChanges { get; set; }
        public long NumberOfLines { get; set; }
    }
}