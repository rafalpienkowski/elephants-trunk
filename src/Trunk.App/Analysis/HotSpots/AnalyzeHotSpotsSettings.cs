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
    [CommandArgument(0, "lines of code file path")]
    public string LinesOfCodeFilePath { get; set; }

    [CommandArgument(1, "code frequencies file path")]
    public string CodeFrequenciesFilePath { get; set; }
}