using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.Extensions;

namespace Trunk.App.Dimensions.Frequencies;

public class MeasureFrequencyOfChangesSettings : CommandSettings
{
    [Description("Specifies path to the git log file produced by this git command: 'git log --pretty=format:'[%h] %an %ad %s' --date=short --numstat --no-merges'")]
    [CommandArgument(0, "[git-log-file-path]")]
    public string? GitLogFilePath { get; set; }

    public override ValidationResult Validate()
    {
        return GitLogFilePath.ValidateFileArgument(nameof(GitLogFilePath));
    }
}