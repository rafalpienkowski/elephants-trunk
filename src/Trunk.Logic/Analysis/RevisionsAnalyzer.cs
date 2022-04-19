using Trunk.Logic.Models;

namespace Trunk.Logic.Analysis;

/// <summary>
/// Calculates the frequency of changes in a file
/// </summary>
public class RevisionsAnalyzer 
{
    public static List<EntityFrequency> Analyze(IEnumerable<Revision> revisions)
    {
        var revisionGroups = revisions.SelectMany(r => r.FileChanges).GroupBy(fc => fc.FilePath)
            .Select(g => new EntityFrequency(g.Key, g.Count())).OrderByDescending(r => r.RevisionNumber).ToList();

        return revisionGroups;
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