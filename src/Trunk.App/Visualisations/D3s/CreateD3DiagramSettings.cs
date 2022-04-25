using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;

namespace Trunk.App.Visualisations.D3s;

public class CreateD3DiagramSettings : CommandSettings
{
    /// <summary>
    /// Path to the repository required for file lines calculation:
    /// </summary>
    [Description("Path to the file containing hot spots analyze results")]
    [CommandArgument(0, "[hot spots file path]")]
    public string? HotSpotsFilePath { get; init; }

    public override ValidationResult Validate()
    {
        return HotSpotsFilePath.ValidateFileArgument(nameof(HotSpotsFilePath));
    }
}