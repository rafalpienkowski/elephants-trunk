using System.Text.RegularExpressions;

namespace Trunk.Logic.Models;

/// <summary>
/// Represents single file change in a revision
/// </summary>
public class FileChange
{
    public uint LinesAdded { get; }
    public uint LinesRemoved { get; }
    public string FilePath { get; }

    private FileChange(uint linesAdded, uint linesRemoved, string filePath)
    {
        LinesAdded = linesAdded;
        LinesRemoved = linesRemoved;
        FilePath = filePath;
    }

    public static FileChange From(Match match) =>
        new(IsEmptyChange(match.Groups[1].Value) ? 0 : uint.Parse(match.Groups[1].Value),
            IsEmptyChange(match.Groups[2].Value) ? 0 : uint.Parse(match.Groups[2].Value), 
            match.Groups[3].Value.Trim());

    private static bool IsEmptyChange(string change) => change.Equals("-");
}