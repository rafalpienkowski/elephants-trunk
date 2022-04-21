using Spectre.Console.Cli;

/// <summary>
/// Settings required for calculation
/// </summary>
public class CalculateSettings : CommandSettings
{
    /// <summary>
    /// Path to the file containing git log produced by this git command:
    /// </summary>
    [CommandArgument(0, "<GIT LOG>")]
    public string? GitLogFilePath { get; set; }
}