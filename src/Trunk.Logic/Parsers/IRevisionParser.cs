using Trunk.Logic.Models;

namespace Trunk.Logic.Parsers;

/// <summary>
/// Parses a revision from a source control log
/// </summary>
public interface IRevisionParser
{
    /// <summary>
    /// Stream containing source control log
    /// </summary>
    /// <param name="streamReader"><see cref="Stream"/></param>
    /// <returns><see cref="Revision"/></returns>
    Task<List<Revision>> ParseAsync(StreamReader streamReader);
}