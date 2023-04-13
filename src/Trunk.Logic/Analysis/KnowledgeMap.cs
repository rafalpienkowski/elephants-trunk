namespace Trunk.Logic.Analysis;

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