using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Complexities;

namespace Trunk.App.Dimensions.Complexities;

public class MeasureCodeLinesCommand : AsyncCommand<MeasureCodeLinesSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, MeasureCodeLinesSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            ctx.Status("Counting lines of code");
            var codeLinesCollection = LinesOfCodeMeasurement.Measure(settings.RootPath);
            
            ctx.Status("Saving result to output");
            await SaveOutput(settings, codeLinesCollection);
        });

        return 0;
    }

    private static async Task SaveOutput(MeasureCodeLinesSettings settings, List<CodeLines> codeLinesCollection)
    {
        if (File.Exists(settings.OutputFileName))
        {
            File.Delete(settings.OutputFileName);
        }

        await using var sw = new StreamWriter(settings.OutputFileName);
        await sw.WriteLineAsync("path,lines");
        foreach (var codeLine in codeLinesCollection)
        {
            await sw.WriteLineAsync($"{codeLine.Path},{codeLine.Lines}");
        }
    }
}