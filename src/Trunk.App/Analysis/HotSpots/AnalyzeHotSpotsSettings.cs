using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Trunk.App.Analysis.HotSpots;

/// <summary>
/// Settings required for calculation
/// </summary>
public class AnalyzeHotSpotsSettings : CommandSettings 
{
    /// <summary>
    /// Path to the repository required for file lines calculation:
    /// </summary>
    [Description("Path to the file contining line of codes measurment")]
    [CommandArgument(0, "lines of code file path")]
    public string? LinesOfCodeFilePath { get; init; }
    
    [Description("Path to the file containing code frequencies measurment")]
    [CommandArgument(1, "code frequencies file path")]
    public string? CodeFrequenciesFilePath { get; init; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(LinesOfCodeFilePath))
        {
            return ValidationResult.Error("Specify lines of code file path");
        }

        if (string.IsNullOrEmpty(CodeFrequenciesFilePath))
        {
            return ValidationResult.Error("Specify code frequency file path");
        }

        if (!File.Exists(LinesOfCodeFilePath))
        {
            return ValidationResult.Error($"File: '{LinesOfCodeFilePath}' does not exist");
        }

        if (!File.Exists(CodeFrequenciesFilePath))
        {
            return ValidationResult.Error($"File: '{CodeFrequenciesFilePath}' does not exist");
        }
        
        return ValidationResult.Success();
    }
}