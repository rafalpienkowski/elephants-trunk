using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;

namespace Trunk.App.Analysis.WordMaps;

public class AnalyzeWordMapCommand : AsyncCommand<AnalyzeWordMapSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, AnalyzeWordMapSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Thinking...", async ctx =>
        {
            ctx.SetupSpinner();
            
            
        });

        return 0;
    }
}