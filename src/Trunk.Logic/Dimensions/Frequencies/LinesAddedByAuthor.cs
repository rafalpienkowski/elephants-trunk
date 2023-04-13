namespace Trunk.Logic.Dimensions.Frequencies;

public class LinesAddedByAuthor
{
    public string Directory { get; }
    public string Author { get; }
    public long NumberOfLines { get; }

    private LinesAddedByAuthor(string directory, string author, long numberOfLines)
    {
        Directory = directory;
        Author = author;
        NumberOfLines = numberOfLines;
    }

    public static LinesAddedByAuthor From(string directory, string author, long numberOfLines) =>
        new(directory, author, numberOfLines);
}