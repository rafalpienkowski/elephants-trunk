using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Extensions;

namespace Trunk.App.Visualisations.D3s;

public class CreateD3DiagramCommand : AsyncCommand<CreateD3DiagramSettings>
{
    private const string OutputFileName = "hotspot_proto.json";
    
    public override async Task<int> ExecuteAsync(CommandContext context, CreateD3DiagramSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
            
            ctx.Status("Loading hot spots file");
            var hotSpots = settings.HotSpotsFilePath!.ReadFromCsvFile<HotSpotModel>().ToList();

            ctx.Status("Transforming data");
            var results = TransformData(hotSpots);

            ctx.Status("Saving results");
            await OutputFileName.WriteToJsonFile(results);
        });

        AnsiConsole.Write($"Analyze finished. Results exported to: '{OutputFileName}'");
        return 0;
    }

    private static Node TransformData(List<HotSpotModel> hotSpotModels)
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
    public class Node
    {
        public string Name { get; set; } = null!;
        public List<Node> Children { get; set; } = new();
    }

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