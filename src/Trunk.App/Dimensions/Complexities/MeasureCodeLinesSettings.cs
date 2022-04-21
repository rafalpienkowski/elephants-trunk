using Spectre.Console.Cli;

namespace Trunk.App.Dimensions.Complexities;

public class MeasureCodeLinesSettings : CommandSettings
{
    /// <summary>
    /// Root path for the measurement
    /// </summary>
    [CommandArgument(0, "[root-path]")] 
    public string RootPath { get; set; } = ".";

    /// <summary>
    /// Output's file name
    /// </summary>
    [CommandOption("-o|--output")] 
    public string OutputFileName { get; set; } = "lines-of-code.csv";
}