using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Complexities;

namespace Trunk.App.Dimensions;

public class MeasureCodeLinesSettings : CommandSettings
{
    /// <summary>
    /// Root path for the measurement
    /// </summary>
    [Description("Root path")]
    [CommandArgument(0, "[root-path]")]
    public string RootPath { get; set; } = ".";

    public override ValidationResult Validate()
    {
        return !Directory.Exists(RootPath)
            ? ValidationResult.Error($"Root directory: '{RootPath}' does not exist")
            : ValidationResult.Success();
    }
}

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