using Trunk.Logic.Analysis;

namespace Trunk.Logic.Visualizations;

public static class HotSpotsVisualizer
{
    public static Node TransformData(List<HotSpot> hotSpotModels)
    {
        var root = new Node
        {
            Name = "root"
        };

        var maxNumberOfChanges = hotSpotModels.Max(h => h.NumberOfChanges);
        foreach (var hotSpotModel in hotSpotModels)
        {
            var modules = hotSpotModel.File.Split('/');
            var node = new Leaf
            {
                Name = modules.Last(),
                Size = hotSpotModel.NumberOfLines.ToString(),
                Weight = (decimal)hotSpotModel.NumberOfChanges / maxNumberOfChanges
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

                var newNode = new Node
                {
                    Name = modules[i]
                };
                currentNode.Children.Add(newNode);
                currentNode = newNode;
            }
            
            currentNode.Children.Add(node);
        }

        return root;
    }
}

/// <summary>
/// Represents a node used for D3 visualization library
/// </summary>
public class Node
{
    public string Name { get; set; } = null!;
    public List<Node> Children { get; set; } = new();
}

/// <summary>
/// Represents a leaf used for D3 visualization library
/// </summary>
public class Leaf : Node
{
    /// <summary>
    /// Number of lines
    /// </summary>
    public string Size { get; set; } = null!;

    /// <summary>
    /// Normalized [0..1] number of changes
    /// </summary>
    public decimal Weight { get; set; }
}
