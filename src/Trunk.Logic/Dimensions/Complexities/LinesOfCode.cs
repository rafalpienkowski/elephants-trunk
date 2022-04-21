using Easy.Common.Extensions;

namespace Trunk.Logic.Dimensions.Complexities;

/// <summary>
/// Calculates number of code lines
/// </summary>
public static class LinesOfCode
{
    /// <summary>
    /// Measures recursively code lines in files under given path
    /// </summary>
    /// <param name="path">Path to analyze</param>
    /// <returns><see cref="CodeLines"/></returns>
    public static List<CodeLines> Measure(string path)
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