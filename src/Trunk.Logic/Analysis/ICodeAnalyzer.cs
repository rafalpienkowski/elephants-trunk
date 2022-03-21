using Trunk.Logic.Models;

namespace Trunk.Logic.Analysis;

/// <summary>
/// Analyse code
/// </summary>
public interface ICodeAnalyzer
{
    /// <summary>
    /// Analyse code
    /// </summary>
    /// <param name="revisions"><see cref="Revision"/></param>
    /// <returns><see cref="IAnalysisResult"/></returns>
    IAnalysisResult Analyze(IList<Revision> revisions);
}
