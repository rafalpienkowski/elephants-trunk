using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Trunk.App.Dimensions.Frequencies;

public class MeasureFrequencyOfChangesSettings : CommandSettings
{
    [Description("Specifies path to the git log file produced by this git command: 'git log --pretty=format:'[%h] %an %ad %s' --date=short --numstat --no-merges'")]
    [CommandArgument(0, "[git-log-file-path]")]
    public string? GitLogFilePath { get; set; }
    
    /// <summary>
    /// Output's file name
    /// </summary>
    [Description("Specifies output file name. Default value: 'frequencies.csv'")]
    [CommandOption("-o|--output")]
    [DefaultValue("freqiencies.csv")]
    public string? OutputFileName { get; init; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(OutputFileName))
        {
            return ValidationResult.Error("Specify output file name");
        }

        if (string.IsNullOrEmpty(GitLogFilePath))
        {
            return ValidationResult.Error("Specify git log file path");
        }

        if (!File.Exists(GitLogFilePath))
        {
            return ValidationResult.Error($"File: '{GitLogFilePath}' does not exist");
        }
        
        return ValidationResult.Success();
    }
}