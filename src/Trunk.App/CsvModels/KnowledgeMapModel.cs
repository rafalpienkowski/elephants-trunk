using Trunk.Logic.Analysis;

namespace Trunk.App.CsvModels;

public class KnowledgeMapModel
{
    public string File { get; set; } = null!;
    public string Author { get; set; } = null!;
    public long NumberOfLines { get; set; }

    public static KnowledgeNode ToEntity(KnowledgeMapModel model) => new(model.File, model.Author, model.NumberOfLines);
}