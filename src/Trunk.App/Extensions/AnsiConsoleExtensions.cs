using Spectre.Console;

namespace Trunk.App.Extensions;

public static class AnsiConsoleExtensions
{
    public static void SetupSpinner(this StatusContext ctx)
    {
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));
    }
}