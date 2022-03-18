using System.Text.RegularExpressions;

namespace Trunk.Logic.Models;

/// <summary>
/// Represents single code revision
/// </summary>
public class Revision
{
    public string CommitHash { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; }
    public List<FileChange> FileChanges { get; set; } = new();

    public static Revision From(Match match)
    {
        return new Revision
        {
            CommitHash = match.Groups[1].Value.Trim(),
            Author = match.Groups[2].Value.Trim(),
            Date = DateTime.Parse(match.Groups[3].Value),
            Message = match.Groups[4].Value.Trim()
        };
    }
}