using Spectre.Console.Cli;

namespace Trunk.App.Dimensions.Frequencies;

public class MeasureFrequencyOfChangesSettings : CommandSettings
{
    [CommandArgument(0, "[git-log-file-path]")]
    public string? GitLogFilePath { get; set; }
    
    /// <summary>
    /// Output's file name
    /// </summary>
    [CommandOption("-o|--output")] 
    public string OutputFileName { get; set; } = "frequency.csv";
}