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
    [DefaultValue(".")]
    public string? RootPath { get; set; }

    /// <summary>
    /// Output's file name
    /// </summary>
    [Description("Specifies output file name. Default value: 'lines-of-code.csv'")]
    [CommandOption("-o|--output")]
    [DefaultValue("lines-of-code.csv")]
    public string? OutputFileName { get; set; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(OutputFileName))
        {
            return ValidationResult.Error("Specify output file name");
        }

        if (string.IsNullOrEmpty(RootPath))
        {
            return ValidationResult.Error("Specify git log file path");
        }

        if (!Directory.Exists(RootPath))
        {
            return ValidationResult.Error($"Root directory: '{RootPath}' does not exist");
        }

        return ValidationResult.Success();
    }
}