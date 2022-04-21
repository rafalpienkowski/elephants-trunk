using Easy.Common.Extensions;

namespace Trunk.Logic.Dimensions.Complexities;

/// <summary>
/// Calculates number of code lines
/// </summary>
public class LinesOfCode
{
    /// <summary>
    /// Counts recursively code lines in files under given path
    /// </summary>
    /// <param name="path">Path to analyze</param>
    /// <returns><see cref="CodeLines"/></returns>
    public static List<CodeLines> Count(string path)
    {
        var files = new List<CodeLines>();

        if (!Directory.Exists(path))
        {
            throw new ArgumentOutOfRangeException(nameof(path), "Given directory does not exist");
        }

        CountLines(path, files, path);

        return files;
    }

    private static void CountLines(string path, ICollection<CodeLines> files, string rootPath)
    {
        var pathLength = rootPath.Length + 1;
        foreach (var file in Directory.GetFiles(path))
        {
            var lines = File.OpenRead(file).CountLines();
            files.Add(CodeLines.From(file[pathLength..], lines));
        }
        
        foreach (var directory in Directory.GetDirectories(path).Where(d => !d.EndsWith(".git")))
        {
            CountLines(directory, files, rootPath);
        }
    }
}

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