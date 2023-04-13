using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.Logic.Analysis;


public static class KnowledgeMapAnalyzer
{
    public static List<KnowledgeNode> CalculateKnowledgeMap(List<AuthorsCodeLinesAdded> authorsCodeLinesAdded)
    {
        var knowledgeMapList = new List<KnowledgeNode>();
        foreach (var codeLinesAdded in authorsCodeLinesAdded)
        {
            foreach (var codeAddedToDir in codeLinesAdded.CodeAddedToDirs)
            {
                var knowledgeMap = knowledgeMapList.SingleOrDefault(km => km.Directory == codeAddedToDir.Path);
                if (knowledgeMap == null)
                {
                    knowledgeMapList.Add(new KnowledgeNode(codeAddedToDir.Path, codeLinesAdded.Author, codeAddedToDir.NumberOfLinesAdded));
                    continue;
                }

                if (knowledgeMap.Author == codeLinesAdded.Author)
                {
                    continue;
                }

                if (knowledgeMap.NumberOfLines < codeAddedToDir.NumberOfLinesAdded)
                {
                    knowledgeMapList.Remove(knowledgeMap);
                    knowledgeMapList.Add(new KnowledgeNode(codeAddedToDir.Path, codeLinesAdded.Author, codeAddedToDir.NumberOfLinesAdded));
                }
            }
        }
        
        return knowledgeMapList;
    }
}

public class KnowledgeNode
{
    public string Directory { get; } 
    public string Author { get; } 
    public long NumberOfLines { get;}

    public KnowledgeNode(string directory, string author, long numberOfLines)
    {
        Directory = directory;
        Author = author;
        NumberOfLines = numberOfLines;
    }
}