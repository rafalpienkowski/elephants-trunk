using System.Text.RegularExpressions;
using Trunk.Logic.Models;

namespace Trunk.Logic.Parsers;

/// <summary>
/// Parses git log to revisions
/// </summary>
internal class GitRevisionParser : IRevisionParser
{
    public async Task<List<Revision>> ParseAsync(StreamReader streamReader)
    {
        var revisions = new List<Revision>();
        string line;
        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            var regex = new Regex(@"^\[([0-9A-Fa-f]{5,12})\].(.*?)([0-9]{4}-[0-9]{2}-[0-9]{2}).(.*)");
            var match = regex.Match(line);
            if (match.Success)
            {
                var revision = new Revision
                {
                    CommitHash = match.Groups[1].Value.Trim(),
                    Author = match.Groups[2].Value.Trim(),
                    Date = DateTime.Parse(match.Groups[3].Value),
                    Message = match.Groups[4].Value.Trim()
                };
                
                while(!string.IsNullOrEmpty(line = await streamReader.ReadLineAsync()))
                {
                    var regex2 = new Regex(@"(\d*)\t(\d*)\t(.*)");
                    var match2 = regex2.Match(line);
                    if (match2.Success)
                    {
                        var fileChange = new FileChange
                        {
                            LinesAdded = uint.Parse(match2.Groups[1].Value),
                            LinesRemoved = uint.Parse(match2.Groups[2].Value),
                            FilePath = match2.Groups[3].Value.Trim()
                        };
                        
                        revision.FileChanges.Add(fileChange);
                    }
                }
                
                revisions.Add(revision);
            }
        }

        return revisions;
    }
}