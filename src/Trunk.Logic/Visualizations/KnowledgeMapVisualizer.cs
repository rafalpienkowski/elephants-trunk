using Trunk.Logic.Analysis;

namespace Trunk.Logic.Visualizations;

public static class KnowledgeMapVisualizer
{
    private static readonly string[] BasicColors = {
        "silver",
        "gray",
        "maroon",
        "red",
        "purple",
        "fuchsia",
        "green",
        "lime",
        "olive",
        "yellow",
        "navy",
        "blue",
        "teal",
        "aqua"
    };
    
    public static (KnowledgeMapNode, KeyValuePair<string,string>[]) TransformData(List<KnowledgeMap> knowledgeMap)
    {
        var root = new KnowledgeMapNode
        {
            Name = "root"
        };

        var random = new Random();
        var authors = knowledgeMap.Select(km => km.Author).Distinct();
        var authorColorsMap = authors.ToDictionary(author => author, _ => BasicColors[random.Next(0, BasicColors.Length - 1)]);

        foreach (var knowledgeNode in knowledgeMap)
        {
            var modules = knowledgeNode.File.Split('/');
            var node = new KnowledgeMapLeaf
            {
                Name = modules.Last(),
                Size = knowledgeNode.NumberOfLines.ToString(),
                Author = knowledgeNode.Author,
                Color = authorColorsMap[knowledgeNode.Author],
            };
            node.Children.Add(new KnowledgeMapLeaf
            {
                Name = modules.Last(),
                Size = knowledgeNode.NumberOfLines.ToString(),
                Author = knowledgeNode.Author,
                Color = authorColorsMap[knowledgeNode.Author],
            });

            root.Children.Add(node);
        }

        return (root, authorColorsMap.ToArray());
    }
}

/// <summary>
/// Represents a node used for D3 visualization library
/// </summary>
public class KnowledgeMapNode
{
    public string Name { get; set; } = null!;
    public List<KnowledgeMapNode> Children { get; set; } = new();
}

/// <summary>
/// Represents a leaf used for D3 visualization library
/// </summary>
public class KnowledgeMapLeaf : KnowledgeMapNode
{
    /// <summary>
    /// Number of lines
    /// </summary>
    public string Size { get; set; } = null!;

    /// <summary>
    /// Author
    /// </summary>
    public string Author { get; set; } = null!;

    /// <summary>
    /// Color assigned to author
    /// </summary>
    public string Color { get; set; } = null!;
}