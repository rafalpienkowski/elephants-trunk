namespace Trunk.Logic.Loaders;

/// <summary>
/// Loads revisions to a stream
/// </summary>
public interface ISourceControlLogLoader
{
    /// <summary>
    /// Loads source control log to a stream
    /// </summary>
    /// <returns></returns>
    Task<StreamReader> LoadAsync();
}