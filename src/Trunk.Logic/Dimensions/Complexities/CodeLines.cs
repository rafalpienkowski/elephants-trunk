namespace Trunk.Logic.Dimensions.Complexities;

/// <summary>
/// Number of lines in single file
/// </summary>
public class CodeLines
{
    public string Path { get; }
    public long Lines { get; }

    private CodeLines(string path, long lines)
    {
        Path = path;
        Lines = lines;
    }

    public static CodeLines From(string path, long lines) => new(path.Replace(@"\",@"/"), lines);
}