using Trunk.Logic.Analysis;

namespace Trunk.App.CsvModels;

public class HotSpotModel
{
    public string File { get; set; } = null!;
    public long NumberOfChanges { get; set; }
    public long NumberOfLines { get; set; }

    public static HotSpot ToEntity(HotSpotModel model) =>
        new HotSpot(model.File, model.NumberOfChanges, model.NumberOfLines);
}