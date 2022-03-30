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

        CountLines(path, files);

        return files;
    }

    private static void CountLines(string path, List<FileLines> files)
    {
        foreach (var file in Directory.GetFiles(path))
        {
            var lines = File.OpenRead(file).CountLines();
            files.Add(new FileLines(file, lines));

            foreach (var directory in Directory.GetDirectories(path))
            {
                CountLines(directory, files);
            }
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

    public FileLines(string path, long lines)
    {
        Path = path;
        Lines = lines;
    }
}