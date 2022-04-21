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