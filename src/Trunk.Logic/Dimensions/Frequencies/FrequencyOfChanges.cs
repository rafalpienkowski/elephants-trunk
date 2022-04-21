namespace Trunk.Logic.Dimensions.Frequencies;

/// <summary>
/// Represents number of changes for a given file
/// </summary>
public class FrequencyOfChanges
{
    public string Path { get; }
    public int NumberOfChanges { get; }

    private FrequencyOfChanges(string path, int numberOfChanges)
    {
        Path = path;
        NumberOfChanges = numberOfChanges;
    }

    public static FrequencyOfChanges From(string path, int numberOfChanges) => new(path, numberOfChanges);
}