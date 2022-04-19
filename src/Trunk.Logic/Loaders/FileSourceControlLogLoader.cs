namespace Trunk.Logic.Loaders;

/// <summary>
/// Loads source control log from a file
/// </summary>
public class FileSourceControlLogLoader 
{
    private readonly string _filePath;

    public FileSourceControlLogLoader(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Loads source control log to a stream
    /// </summary>
    /// <returns></returns>
    public Task<StreamReader> LoadAsync()
    {
        return Task.FromResult(File.OpenText(_filePath));
    }
}