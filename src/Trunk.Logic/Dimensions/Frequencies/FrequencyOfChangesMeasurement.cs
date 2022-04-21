namespace Trunk.Logic.Dimensions.Frequencies;

/// <summary>
/// Represents number of changes for a given file
/// </summary>
public class FrequencyOfChangesMeasurement
{
    public string Path { get; }
    public int NumberOfChanges { get; }

    private FrequencyOfChangesMeasurement(string path, int numberOfChanges)
    {
        Path = path;
        NumberOfChanges = numberOfChanges;
    }

    public static FrequencyOfChangesMeasurement From(string path, int numberOfChanges) => new(path, numberOfChanges);
}