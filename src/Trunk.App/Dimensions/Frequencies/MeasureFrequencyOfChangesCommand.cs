using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Frequencies;
using Trunk.Logic.Loaders;
using Trunk.Logic.Parsers;

namespace Trunk.App.Dimensions.Frequencies;

public class MeasureFrequencyOfChangesCommand : AsyncCommand<MeasureFrequencyOfChangesSettings>
{
    private const string OutputFileName = "frequencies.csv";
    
    public override async Task<int> ExecuteAsync(CommandContext context, MeasureFrequencyOfChangesSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
            
            ctx.Status("Loading git log file");
            var revisions = await LoadRevisions(settings.GitLogFilePath!);

            ctx.Status("Counting file change frequencies");
            var frequencies = ChangesInFileMeasurement.Measure(revisions);
            
            ctx.Status("Saving result to output");
            await OutputFileName.WriteToCsvFile(frequencies);

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