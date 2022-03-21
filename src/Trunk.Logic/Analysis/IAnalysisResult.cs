namespace Trunk.Logic.Analysis;

/// <summary>
/// Analysis result
/// </summary>
public interface IAnalysisResult
{
    /// <summary>
    /// Formats the result to CSV output
    /// </summary>
    /// <returns>A string containing CSV result</returns>
    string ToCsv();
}