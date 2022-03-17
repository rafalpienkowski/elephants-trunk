namespace Trunk.Logic.Models;

/// <summary>
/// Represents single file change in a revision
/// </summary>
public class FileChange
{
    public uint LinesAdded { get; set; }
    public uint LinesRemoved { get; set; }
    public string FilePath { get; set; }
}