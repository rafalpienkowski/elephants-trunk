using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Frequencies;
using Trunk.Logic.Loaders;
using Trunk.Logic.Parsers;

namespace Trunk.App.Dimensions.Frequencies;

public class MeasureFrequencyOfChangesCommand : AsyncCommand<MeasureFrequencyOfChangesSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, MeasureFrequencyOfChangesSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();
            
            ctx.Status("Loading git log file");
            var revisions = await LoadRevisions(settings);

            ctx.Status("Counting file change frequencies");
            var frequencies = ChangesInFileMeasurement.Measure(revisions);
            
            ctx.Status("Saving result to output");
            await SaveOutput(settings, frequencies);

        });

        return 0;
    }

    private static async Task<List<Revision>> LoadRevisions(MeasureFrequencyOfChangesSettings settings)
    {
        if (string.IsNullOrEmpty(settings.GitLogFilePath))
        {
            throw new ArgumentOutOfRangeException(nameof(settings.GitLogFilePath), "Specify git log file path");
        }
            
        var loader = new FileSourceControlLogLoader(settings.GitLogFilePath);
        using var streamReader = await loader.LoadAsync();
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        return revisions;
    }

    private static async Task SaveOutput(MeasureFrequencyOfChangesSettings settings, List<FrequencyOfChanges> frequencyOfChanges)
    {
        if (File.Exists(settings.OutputFileName))
        {
            File.Delete(settings.OutputFileName);
        }

        await using var sw = new StreamWriter(settings.OutputFileName);
        await sw.WriteLineAsync("path,number of changes");
        foreach (var changes in frequencyOfChanges)
        {
            await sw.WriteLineAsync($"{changes.Path},{changes.NumberOfChanges}");
        }
    }
}