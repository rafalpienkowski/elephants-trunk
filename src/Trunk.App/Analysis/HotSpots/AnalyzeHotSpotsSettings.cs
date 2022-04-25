using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;

namespace Trunk.App.Analysis.HotSpots;

/// <summary>
/// Settings required for calculation
/// </summary>
public class AnalyzeHotSpotsSettings : CommandSettings 
{
    /// <summary>
    /// Path to the repository required for file lines calculation:
    /// </summary>
    [Description("Path to the file containing line of codes measurement")]
    [CommandArgument(0, "[lines of code file path]")]
    public string? LinesOfCodeFilePath { get; init; }
    
    [Description("Path to the file containing code frequencies measurment")]
    [CommandArgument(1, "[code frequencies file path]")]
    public string? CodeFrequenciesFilePath { get; init; }

    public override ValidationResult Validate()
    {
        var validationResult = LinesOfCodeFilePath.ValidateFileArgument(nameof(LinesOfCodeFilePath));
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return CodeFrequenciesFilePath.ValidateFileArgument(nameof(CodeFrequenciesFilePath));
    }
}