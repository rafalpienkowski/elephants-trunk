using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.App.CsvModels;

public class LinesAddedByAuthorModel
{
    public string Directory { get; set; } = null!;
    public string Author { get; set; } = null!;
    public long NumberOfLines { get; set; }

    public static LinesAddedByAuthor ToEntity(LinesAddedByAuthorModel model) =>
        LinesAddedByAuthor.From(model.Directory, model.Author, model.NumberOfLines);
}