using Trunk.Logic.Dimensions.Complexities;

namespace Trunk.App.CsvModels;

public class CodeLinesModel
{
    public string Path { get; set; } = null!;
    public long Lines { get; set; }
    
    public static CodeLines ToEntity(CodeLinesModel model) => CodeLines.From(model.Path, model.Lines);
}