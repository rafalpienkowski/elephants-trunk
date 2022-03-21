using System.Text;
using Trunk.Logic.Models;

namespace Trunk.Logic.Analysis;

/// <summary>
/// Calculates a summary from a log
/// </summary>
public class SummaryAnalyzer : ICodeAnalyzer
{
    public IAnalysisResult Analyze(IList<Revision> revisions)
    {
        var commits = revisions.Count;
        var filesChanged = revisions.SelectMany(r => r.FileChanges).Select(file => file.FilePath).ToList();
        var entities = filesChanged.Distinct().Count();
        var entitiesChanged = filesChanged.Count;
        var authors = revisions.Select(r => r.Author).Distinct().Count();

        return new SummaryAnalysisResult(commits, entities, entitiesChanged, authors);
    }
}

public class SummaryAnalysisResult : IAnalysisResult
{
    public int NumberOfCommits { get; }
    public int NumberOfEntities { get; }
    public int NumberOfEntitiesChanged { get; }
    public int NumberOfAuthors { get; }

    public SummaryAnalysisResult(int numberOfCommits, int numberOfEntities, int numberOfEntitiesChanged, int numberOfAuthors)
    {
        NumberOfCommits = numberOfCommits;
        NumberOfEntities = numberOfEntities;
        NumberOfEntitiesChanged = numberOfEntitiesChanged;
        NumberOfAuthors = numberOfAuthors;
    }

    public string ToCsv()
    {
        var sb = new StringBuilder("statistic,value");
        
        sb.AppendLine($"number-of-commits,{NumberOfCommits.ToString()}");
        sb.AppendLine($"number-of-entities,{NumberOfEntities.ToString()}");
        sb.AppendLine($"number-of-authors,{NumberOfAuthors.ToString()}");
        sb.AppendLine($"number-of-entity-changed,{NumberOfEntitiesChanged.ToString()}");
        
        return sb.ToString();
    }
}
