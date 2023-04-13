using Trunk.Logic.Analysis;

namespace Trunk.App.CsvModels;

public class KnowledgeNodeModel
{
    public string Directory { get; set; } = null!;
    public string Author { get; set; } = null!;
    public long NumberOfLines { get; set; }

    public static KnowledgeNode ToEntity(KnowledgeNodeModel model) => new(model.Directory, model.Author, model.NumberOfLines);
}