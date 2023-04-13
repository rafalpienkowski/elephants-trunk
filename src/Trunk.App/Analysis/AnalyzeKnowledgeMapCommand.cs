using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Extensions;
using Trunk.Logic.Analysis;

namespace Trunk.App.Analysis;

public class AnalyzeKnowledgeMapSettings : CommandSettings
{

    [Description("Path to file with lines added by author")]
    [CommandArgument(0, "[lines-added-by-author]")]
    public string LinesAddedByAuthor { get; set; } = null!;

    public override ValidationResult Validate()
    {
        return LinesAddedByAuthor.ValidateFileArgument(nameof(LinesAddedByAuthor));
    }
}

public class AnalyzeKnowledgeMapCommand : AsyncCommand<AnalyzeKnowledgeMapSettings>
{
    private const string OutputFileName = "knowledge-nodes.csv";
    
    public override async Task<int> ExecuteAsync(CommandContext context, AnalyzeKnowledgeMapSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Thinking...", async ctx =>
        {
            ctx.SetupSpinner();
            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
        
            ctx.Status("Loading lines added by author file");
            var linesAddedByAuthor = settings.LinesAddedByAuthor.ReadFromCsvFile<LinesAddedByAuthorModel>()
                .Select(LinesAddedByAuthorModel.ToEntity).ToList();
        
            ctx.Status("Analyzing knowledge map");
            var combinedComplexities = KnowledgeMapAnalyzer.CalculateKnowledgeMap(linesAddedByAuthor);
        
            ctx.Status("Saving results");
            await OutputFileName.WriteToCsvFile(combinedComplexities);
        });
        
        AnsiConsole.WriteLine($"Analyze finished. Results exported to: '{OutputFileName}'");
        return 0;
    }
}