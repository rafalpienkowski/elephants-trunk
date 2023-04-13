using Trunk.Logic.Parsers;

namespace Trunk.Logic.Dimensions.Frequencies;

/// <summary>
/// Counts the frequency of changes in a file
/// </summary>
public static class ChangesInFileMeasurement 
{
    /// <summary>
    /// Measures the number of changes in files based on revisions
    /// </summary>
    /// <param name="revisions"><see cref="Revision"/></param>
    /// <returns><see cref="FrequencyOfChanges"/></returns>
    public static List<FrequencyOfChanges> Measure(IEnumerable<Revision> revisions)
    {
        var revisionGroups = revisions.SelectMany(r => r.FileChanges).GroupBy(fc => fc.FilePath)
            .Select(g => FrequencyOfChanges.From(g.Key, g.Count())).OrderByDescending(r => r.NumberOfChanges).ToList();

        return revisionGroups;
    }
}

public static class AuthorsCodeLinesAddedMeasurement
{

    public static List<AuthorsCodeLinesAdded> Measure(IEnumerable<Revision> revisions)
    {
        var authorGroups = revisions
            .SelectMany(r => r.FileChanges, (r, fc) => new { r.Author, fc.FilePath, fc.LinesAdded })
            .GroupBy(x => new { x.Author, x.FilePath })
            .Select(g => new { g.Key.Author, g.Key.FilePath, TotalLinesAdded = g.Sum(x => x.LinesAdded) })
            .ToList();

        var authorsCodeLinesAdded = new List<AuthorsCodeLinesAdded>();

        foreach (var group in authorGroups)
        {
            var author = authorsCodeLinesAdded.SingleOrDefault(a => a.Author == group.Author);
            if (author == null)
            {
                author = AuthorsCodeLinesAdded.From(group.Author);
                authorsCodeLinesAdded.Add(author);
            }
            author.AddLines(group.Author, group.FilePath, group.TotalLinesAdded);
            
        }

        return authorsCodeLinesAdded.Where(a => a.CodeAddedToDirs.Count > 0).ToList();
    }
}