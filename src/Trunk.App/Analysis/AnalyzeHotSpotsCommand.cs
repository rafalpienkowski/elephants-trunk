using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Extensions;
using Trunk.Logic.Analysis;

namespace Trunk.App.Analysis;

/// <summary>
/// Settings required for calculation
/// </summary>
public class AnalyzeHotSpotsSettings : CommandSettings
{
    /// <summary>
    /// Path to the repository required for file lines calculation:
    /// </summary>
    [Description("Path to the file containing line of codes measurement")]
    [CommandArgument(0, "[lines of code file path]")]
    public string? LinesOfCodeFilePath { get; init; }

    [Description("Path to the file containing code frequencies measurment")]
    [CommandArgument(1, "[code frequencies file path]")]
    public string? CodeFrequenciesFilePath { get; init; }

    public override ValidationResult Validate()
    {
        var validationResult = LinesOfCodeFilePath.ValidateFileArgument(nameof(LinesOfCodeFilePath));
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return CodeFrequenciesFilePath.ValidateFileArgument(nameof(CodeFrequenciesFilePath));
    }
}

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
            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }

            ctx.Status("Loading lines of code file");
            var lines = settings.LinesOfCodeFilePath!.ReadFromCsvFile<CodeLinesModel>().Select(CodeLinesModel.ToEntity)
                .ToList();

            ctx.Status("Loading code frequencies file");
            var revisionFrequency =
                settings.CodeFrequenciesFilePath!.ReadFromCsvFile<FrequencyOfChangesModel>()
                    .Select(FrequencyOfChangesModel.ToEntity);

            ctx.Status("Analyzing hot spots");
            var combinedComplexities = HotSpotsAnalyzer.CalculateHotSpots(revisionFrequency, lines);

            ctx.Status("Saving results");
            await OutputFileName.WriteToCsvFile(combinedComplexities);

            PrintTop10HotSpotFiles(combinedComplexities.Take(10).ToList());
        });

        AnsiConsole.Write($"Analyze finished. Results exported to: '{OutputFileName}'");
        return 0;
    }

    private static void PrintTop10HotSpotFiles(IEnumerable<HotSpot> combinedComplexities)
    {
        var table = new Table();
        table.AddColumn("Entity");
        table.AddColumn("Number of changes");
        table.AddColumn("Number of lines");

        foreach (var complexity in combinedComplexities)
        {
            table.AddRow(complexity.File, complexity.NumberOfChanges.ToString(), complexity.NumberOfLines.ToString());
        }

        AnsiConsole.Write(table);
    }
}