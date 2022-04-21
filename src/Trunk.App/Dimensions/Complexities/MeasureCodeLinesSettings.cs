using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Trunk.App.Dimensions.Complexities;

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