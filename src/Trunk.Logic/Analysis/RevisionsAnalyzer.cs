using System.Text;
using Trunk.Logic.Models;

namespace Trunk.Logic.Analysis;

/// <summary>
/// Calculates the frequency of changes in a file
/// </summary>
public class RevisionsAnalyzer : ICodeAnalyzer
{
    public IAnalysisResult Analyze(IList<Revision> revisions)
    {
        var revisionGroups = revisions.SelectMany(r => r.FileChanges).GroupBy(fc => fc.FilePath)
            .Select(g => new EntityFrequency(g.Key, g.Count())).OrderByDescending(r => r.RevisionNumber).ToList();

        return new RevisionAnalysisResult(revisionGroups);
    }
}

public class EntityFrequency
{
    public string Entity { get; }
    public int RevisionNumber { get; }

    public EntityFrequency(string entity, int revisionNumber)
    {
        Entity = entity;
        RevisionNumber = revisionNumber;
    }
}

public class RevisionAnalysisResult : IAnalysisResult
{
    public List<EntityFrequency> EntityFrequencies { get; }

    public RevisionAnalysisResult(List<EntityFrequency> entityFrequencies)
    {
        EntityFrequencies = entityFrequencies;
    }

    public string ToCsv()
    {
        var sb = new StringBuilder("entity,n-revs");
        foreach (var entityFrequency in EntityFrequencies)
        {
            sb.AppendLine($"{entityFrequency.Entity},{entityFrequency.RevisionNumber.ToString()}");
        }
        return sb.ToString();
    }
}
