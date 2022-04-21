using System.Text.RegularExpressions;

namespace Trunk.Logic.Parsers;

/// <summary>
/// Represents single code revision
/// </summary>
public class Revision
{
    public string Author { get; }
    public DateTime Date { get; }
    public string Message { get; }
    public List<FileChange> FileChanges { get; } = new();

    private Revision(string author, DateTime date, string message)
    {
        Author = author;
        Date = date;
        Message = message; 
    }

    public static Revision From(Match match) =>
        new(match.Groups[2].Value.Trim(), DateTime.Parse(match.Groups[3].Value), match.Groups[4].Value.Trim());
}