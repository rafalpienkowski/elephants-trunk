using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Complexities;

namespace Trunk.App.Dimensions.Complexities;

public class MeasureCodeLinesCommand : AsyncCommand<MeasureCodeLinesSettings>
{
    private const string OutputFileName = "lines-of-code.csv";
    
    public override async Task<int> ExecuteAsync(CommandContext context, MeasureCodeLinesSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            ctx.Status("Counting lines of code");
            var codeLinesCollection = LinesOfCodeMeasurement.Measure(settings.RootPath);
            
            ctx.Status("Saving result to output");
            await OutputFileName.WriteToCsvFile(codeLinesCollection);
        });
        
        AnsiConsole.Write($"Measurement finished. Results exported to: '{OutputFileName}'");

        return 0;
    }
}