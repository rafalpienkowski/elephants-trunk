namespace Trunk.Logic.Loaders;

/// <summary>
/// Loads source control log from a file
/// </summary>
internal class FileSourceControlLogLoader : ISourceControlLogLoader
{
    private readonly string _filePath;

    public FileSourceControlLogLoader(string filePath)
    {
        _filePath = filePath;
    }

    public Task<StreamReader> LoadAsync()
    {
        return Task.FromResult(File.OpenText(_filePath));
    }
}