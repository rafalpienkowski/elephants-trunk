using System.Text.RegularExpressions;

namespace Trunk.Logic.Models;

/// <summary>
/// Represents single file change in a revision
/// </summary>
public class FileChange
{
    public uint LinesAdded { get; set; }
    public uint LinesRemoved { get; set; }
    public string FilePath { get; set; }

    public static FileChange From(Match match)
    {
        return new FileChange
        {
            LinesAdded = IsEmptyChange(match.Groups[1].Value) ? 0 : uint.Parse(match.Groups[1].Value),
            LinesRemoved = IsEmptyChange(match.Groups[2].Value) ? 0 : uint.Parse(match.Groups[2].Value),
            FilePath = match.Groups[3].Value.Trim()
        };
    }

    private static bool IsEmptyChange(string change) => change.Equals("-");
}