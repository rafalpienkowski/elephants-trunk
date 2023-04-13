using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.Logic.Analysis;


public static class KnowledgeMapAnalyzer
{
    public static List<KnowledgeMap> CalculateHotSpots(List<AuthorsCodeLinesAdded> authorsCodeLinesAdded)
    {
        return new List<KnowledgeMap>();
    }
}

public class KnowledgeMap
{
    public string File { get; } 
    public string Author { get; } 
    public long NumberOfLines { get;}

    public KnowledgeMap(string file, string author, long numberOfLines)
    {
        File = file;
        Author = author;
        NumberOfLines = numberOfLines;
    }
}