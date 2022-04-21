using Trunk.Logic.Dimensions.Frequencies;

namespace Trunk.App.CsvModels;

public class FrequencyOfChangesModel
{
    public string Path { get; set; } = null!;
    public int NumberOfChanges { get; set; }
    
    public static FrequencyOfChanges ToEntity(FrequencyOfChangesModel model) => FrequencyOfChanges.From(model.Path, model.NumberOfChanges);
}