using Trunk.Logic.Dimensions.Complexities;
using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.Logic.Analysis;

public static class HotSpotsAnalyzer
{
    public static List<HotSpot> CalculateHotSpots(IEnumerable<FrequencyOfChanges> revisionFrequency, IList<CodeLines> lines)
    {
        var combinedComplexities = (from entityFrequencyGroup in revisionFrequency.GroupBy(rf => rf.Path)
                where lines.SingleOrDefault(l => l.Path == entityFrequencyGroup.Key) != null
                select new HotSpot(entityFrequencyGroup.Key, entityFrequencyGroup.Sum(g => g.NumberOfChanges),lines.Single(l => l.Path == entityFrequencyGroup.Key).Lines))
            .OrderByDescending(cc => cc.NumberOfChanges)
            .ThenByDescending(cc => cc.NumberOfLines)
            .ToList();
        
        return combinedComplexities;
    }
}

public class HotSpot
{
    public string File { get; }
    public long NumberOfChanges { get; }
    public long NumberOfLines { get; }

    public HotSpot(string file, long numberOfChanges, long numberOfLines)
    {
        File = file;
        NumberOfChanges = numberOfChanges;
        NumberOfLines = numberOfLines;
    }
}