namespace Trunk.App.CsvModels;

public class HotSpotModel
{
    public string File { get; set; } = null!;
    public long NumberOfChanges { get; set; }
    public long NumberOfLines { get; set; }
}