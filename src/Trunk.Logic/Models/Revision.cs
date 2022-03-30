using System.Text.RegularExpressions;

namespace Trunk.Logic.Models;

/// <summary>
/// Represents single code revision
/// </summary>
public class Revision
{
    public string CommitHash { get; }
    public string Author { get; }
    public DateTime Date { get; }
    public string Message { get; }
    public List<FileChange> FileChanges { get; } = new();

    private Revision(string commitHash, string author, DateTime date, string message)
    {
        CommitHash = commitHash;
        Author = author;
        Date = date;
        Message = message; 
    }

    public static Revision From(Match match) =>
         new(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(),
            DateTime.Parse(match.Groups[3].Value), match.Groups[4].Value.Trim());
}