using System.Text;
using Trunk.Logic.Models;

namespace Trunk.Logic.Analysis;

/// <summary>
/// Calculates a summary from a log
/// </summary>
public class SummaryAnalyzer 
{
    public static RevisionSummary Analyze(IList<Revision> revisions)
    {
        var commits = revisions.Count;
        var filesChanged = revisions.SelectMany(r => r.FileChanges).Select(file => file.FilePath).ToList();
        var entities = filesChanged.Distinct().Count();
        var entitiesChanged = filesChanged.Count;
        var authors = revisions.Select(r => r.Author).Distinct().Count();

        return new RevisionSummary(commits, entities, entitiesChanged, authors);
    }
}

public class RevisionSummary 
{
    public int NumberOfCommits { get; }
    public int NumberOfEntities { get; }
    public int NumberOfEntitiesChanged { get; }
    public int NumberOfAuthors { get; }

    public RevisionSummary(int numberOfCommits, int numberOfEntities, int numberOfEntitiesChanged, int numberOfAuthors)
    {
        NumberOfCommits = numberOfCommits;
        NumberOfEntities = numberOfEntities;
        NumberOfEntitiesChanged = numberOfEntitiesChanged;
        NumberOfAuthors = numberOfAuthors;
    }
}
