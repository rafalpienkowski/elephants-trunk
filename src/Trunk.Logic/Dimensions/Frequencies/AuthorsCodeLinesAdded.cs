namespace Trunk.Logic.Dimensions.Frequencies;

public class AuthorsCodeLinesAdded
{
    public string Author { get; }
    public List<DirectoryCodeLinesAdded> CodeAddedToDirs { get; } = new();

    private AuthorsCodeLinesAdded(string author)
    {
        Author = author;
    }

    public static AuthorsCodeLinesAdded From(string author) => new(author);

    public void AddLines(string author, string path, long linesOfAddedCode)
    {
        if (Author != author)
        {
            throw new ArgumentOutOfRangeException(nameof(author), "Invalid author");
        }
        
        var dirPath = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(dirPath))
        {
            return;
        }

        var directory = CodeAddedToDirs.SingleOrDefault(c => c.Path == dirPath);
        if (directory == null)
        {
            CodeAddedToDirs.Add(DirectoryCodeLinesAdded.From(dirPath, linesOfAddedCode));
        }
        else
        {
            directory.AddNumberOfLines(linesOfAddedCode);
        }
    }
}

public class DirectoryCodeLinesAdded
{
    public string Path { get; }
    public long NumberOfLinesAdded { get; private set; }

    public DirectoryCodeLinesAdded(string path, long numberOfLinesAdded)
    {
        Path = path;
        NumberOfLinesAdded = numberOfLinesAdded;
    }

    public static DirectoryCodeLinesAdded From(string path, long numberOfLinesAdded) => new(path, numberOfLinesAdded);

    public void AddNumberOfLines(long numberOfLines)
    {
        NumberOfLinesAdded += numberOfLines;
    }
}