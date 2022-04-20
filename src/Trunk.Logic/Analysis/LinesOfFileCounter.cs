using Easy.Common.Extensions;

namespace Trunk.Logic.Analysis;

public class LinesOfFileCounter
{
    /// <summary>
    /// Counts recursively number of lines in files on given path
    /// </summary>
    /// <param name="path">Path to analyze</param>
    /// <returns><see cref="FileLines"/></returns>
    public static List<FileLines> Count(string path)
    {
        var files = new List<FileLines>();

        if (!Directory.Exists(path))
        {
            throw new ArgumentOutOfRangeException(nameof(path), "Given directory does not exist");
        }

        CountLines(path, files, path);

        return files;
    }

    private static void CountLines(string path, ICollection<FileLines> files, string rootPath)
    {
        var pathLength = rootPath.Length + 1;
        foreach (var file in Directory.GetFiles(path))
        {
            var lines = File.OpenRead(file).CountLines();
            files.Add(FileLines.From(file[pathLength..], lines));
        }
        
        foreach (var directory in Directory.GetDirectories(path).Where(d => !d.EndsWith(".git")))
        {
            CountLines(directory, files, rootPath);
        }
    }
}

/// <summary>
/// Number of lines in give file
/// </summary>
public class FileLines
{
    public string Path { get; }
    public long Lines { get; }

    private FileLines(string path, long lines)
    {
        Path = path;
        Lines = lines;
    }

    public static FileLines From(string path, long lines) => new FileLines(path.Replace(@"\",@"/"), lines);
}