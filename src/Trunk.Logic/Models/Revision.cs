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
}