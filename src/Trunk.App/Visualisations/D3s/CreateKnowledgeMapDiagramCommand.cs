using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Extensions;
using Trunk.Logic.Visualizations;

namespace Trunk.App.Visualisations.D3s;

public class CreateKnowledgeMapDiagramSettings : CommandSettings
{
    /// <summary>
    /// Path to the knowledge map analysis file:
    /// </summary>
    [Description("Path to the file containing knowledge map analysis result")]
    [CommandArgument(0, "[knowledge map file path]")]
    public string? KnowledgeMapFilePath { get; init; }

    public override ValidationResult Validate()
    {
        return KnowledgeMapFilePath.ValidateFileArgument(nameof(KnowledgeMapFilePath));
    }
}

public class CreateKnowledgeMapDiagramCommand : AsyncCommand<CreateKnowledgeMapDiagramSettings>
{
    private const string OutputFileName = "km_proto.json";
    private const string LegendFileName = "km_legend.json";
    
    public override async Task<int> ExecuteAsync(CommandContext context, CreateKnowledgeMapDiagramSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
            
            ctx.Status("Loading knowledge map file");
            var knowledgeMap = settings.KnowledgeMapFilePath!.ReadFromCsvFile<KnowledgeMapModel>().Select(KnowledgeMapModel.ToEntity).ToList();

            ctx.Status("Transforming data");
            var results = KnowledgeMapVisualizer.TransformData(knowledgeMap);

            ctx.Status("Saving results");
            await OutputFileName.WriteToJsonFile(results.Item1);
            await LegendFileName.WriteToJsonFile(results.Item2);
            
        });

        AnsiConsole.WriteLine($"Visualisation finished. Results exported to: '{OutputFileName}'. Legend exported to: '{LegendFileName}'");
        return 0;
    }
}