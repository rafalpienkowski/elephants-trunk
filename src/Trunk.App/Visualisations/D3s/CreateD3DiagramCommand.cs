using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Extensions;
using Trunk.Logic.Visualizations;

namespace Trunk.App.Visualisations.D3s;

public class CreateD3DiagramSettings : CommandSettings
{
    /// <summary>
    /// Path to the repository required for file lines calculation:
    /// </summary>
    [Description("Path to the file containing hot spots analyze results")]
    [CommandArgument(0, "[hot spots file path]")]
    public string? HotSpotsFilePath { get; init; }

    public override ValidationResult Validate()
    {
        return HotSpotsFilePath.ValidateFileArgument(nameof(HotSpotsFilePath));
    }
}

public class CreateD3DiagramCommand : AsyncCommand<CreateD3DiagramSettings>
{
    private const string OutputFileName = "hotspot_proto.json";
    
    public override async Task<int> ExecuteAsync(CommandContext context, CreateD3DiagramSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
            
            ctx.Status("Loading hot spots file");
            var hotSpots = settings.HotSpotsFilePath!.ReadFromCsvFile<HotSpotModel>().Select(HotSpotModel.ToEntity).ToList();

            ctx.Status("Transforming data");
            var results = HotSpotsVisualizer.TransformData(hotSpots);

            ctx.Status("Saving results");
            await OutputFileName.WriteToJsonFile(results);
        });

        AnsiConsole.Write($"Analyze finished. Results exported to: '{OutputFileName}'");
        return 0;
    }
}