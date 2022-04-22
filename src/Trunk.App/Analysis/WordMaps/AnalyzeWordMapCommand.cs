using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Dimensions.Frequencies;
using Trunk.App.Extensions;
using Trunk.Logic.Loaders;
using Trunk.Logic.Parsers;

namespace Trunk.App.Analysis.WordMaps;

public class AnalyzeWordMapCommand : AsyncCommand<AnalyzeWordMapSettings>
{
    private const string OutputFileName = "words.txt";
    
    public override async Task<int> ExecuteAsync(CommandContext context, AnalyzeWordMapSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Thinking...", async ctx =>
        {
            ctx.SetupSpinner();
            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
            
            ctx.Status("Loading git log file");
            var revisions = await LoadRevisions(settings.GitLogFilePath);

            ctx.Status("Wrapping words");
            var commitMessages = revisions.Select(r => r.Message);

            await OutputFileName.WriteToCsvFile(commitMessages);

        });

        return 0;
    }

    private static async Task<List<Revision>> LoadRevisions(string gitLogFilePath)
    {
        var loader = new FileSourceControlLogLoader(gitLogFilePath);
        using var streamReader = await loader.LoadAsync();
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        return revisions;
    }
}