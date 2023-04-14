using Trunk.Logic.Analysis;

namespace Trunk.Logic.Visualizations;

public static class KnowledgeMapVisualizer
{
    private static readonly string[] BasicColors = {
        "green",
        "silver",
        "maroon",
        "lightpink",
        "blue",
        "red",
        "coral",
        "purple",
        "fuchsia",
        "darkgreen",
        "yellow",
        "olive",
        "navy",
        "aqua",
        "indigo",
        "lime",
        "mistyrose",
        "sienna"
    };
    
    public static (KnowledgeMapNode, KeyValuePair<string,string>[]) TransformData(List<KnowledgeNode> knowledgeMap)
    {
        var root = new KnowledgeMapNode
        {
            Name = "root"
        };

        var authors = knowledgeMap.Select(km => km.Author).Distinct();
        var idx = 0;
        var authorColorsMap = authors.ToDictionary(author => author, _ => BasicColors[idx++]);

        foreach (var knowledgeNode in knowledgeMap)
        {
            var modules = knowledgeNode.Directory.Split('/');
            var node = new KnowledgeMapLeaf
            {
                Name = modules.Last(),
                Size = knowledgeNode.NumberOfLines.ToString(),
                Author = knowledgeNode.Author,
                Color = authorColorsMap[knowledgeNode.Author],
            };
            
            var currentNode = root;
            for (var i = 0; i < modules.Length - 1; i++)
            {
                var child = currentNode.Children.FirstOrDefault(n => n.Name == modules[i]);
                if (child != null)
                {
                    currentNode = child;
                    continue;
                }

                var newNode = new KnowledgeMapNode
                {
                    Name = modules[i]
                };
                currentNode.Children.Add(newNode);
                currentNode = newNode;
            }
            
            currentNode.Children.Add(node);
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