using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.Logic.Analysis;

public static class KnowledgeMapAnalyzer
{
    public static List<KnowledgeNode> CalculateKnowledgeMap(List<LinesAddedByAuthor> linesAddedByAuthors)
    {
        var knowledgeMapList = new List<KnowledgeNode>();
        foreach (var codeLinesAdded in linesAddedByAuthors)
        {
            var knowledgeMap = knowledgeMapList.SingleOrDefault(km => km.Directory == codeLinesAdded.Directory);
            if (knowledgeMap == null)
            {
                knowledgeMapList.Add(new KnowledgeNode(codeLinesAdded.Directory, codeLinesAdded.Author,
                    codeLinesAdded.NumberOfLines));
                continue;
            }

            if (knowledgeMap.Author == codeLinesAdded.Author)
            {
                continue;
            }

            if (knowledgeMap.NumberOfLines < codeLinesAdded.NumberOfLines)
            {
                knowledgeMapList.Remove(knowledgeMap);
                knowledgeMapList.Add(new KnowledgeNode(codeLinesAdded.Directory, codeLinesAdded.Author,
                    codeLinesAdded.NumberOfLines));
            }
        }

        return knowledgeMapList;
    }
}

public class KnowledgeNode
{
    public string Directory { get; }
    public string Author { get; }
    public long NumberOfLines { get; }

    public KnowledgeNode(string directory, string author, long numberOfLines)
    {
        Directory = directory;
        Author = author;
        NumberOfLines = numberOfLines;
    }
}