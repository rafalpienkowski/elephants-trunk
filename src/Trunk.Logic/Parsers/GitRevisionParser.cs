using System.Text.RegularExpressions;
using Trunk.Logic.Models;

namespace Trunk.Logic.Parsers;

/// <summary>
/// Parses git log to revisions
/// </summary>
public class GitRevisionParser
{   
    private static readonly Regex RevisionRegex = new(@"^\[([0-9A-Fa-f]{5,12})\].(.*?)([0-9]{4}-[0-9]{2}-[0-9]{2}).(.*)",
        RegexOptions.Singleline | RegexOptions.Compiled, TimeSpan.FromSeconds(5));

    private static readonly Regex FileChangeRegex = new(@"(\d*|-)\t(\d*|-)\t(.*)",
        RegexOptions.Singleline | RegexOptions.Compiled, TimeSpan.FromSeconds(5));
    
    /// <summary>
    /// Stream containing source control log
    /// </summary>
    /// <param name="streamReader"><see cref="Stream"/></param>
    /// <returns><see cref="Revision"/></returns>
    public async Task<List<Revision>> ParseAsync(StreamReader streamReader)
    {
        var revisions = new List<Revision>();
        string line;
        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            var revisionMatch = RevisionRegex.Match(line);
            if (!revisionMatch.Success)
            {
                continue;
            }
            
            var revision = Revision.From(revisionMatch);
            
            while(!string.IsNullOrEmpty(line = await streamReader.ReadLineAsync()))
            {
                var fileChangeMatch = FileChangeRegex.Match(line);
                if (!fileChangeMatch.Success)
                {
                    continue;
                }
                
                var fileChange = FileChange.From(fileChangeMatch);
                revision.FileChanges.Add(fileChange);
            }
                
            revisions.Add(revision);
        }

        return revisions;
    }
}