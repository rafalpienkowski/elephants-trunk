using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using Trunk.App.CsvModels;
using Trunk.App.Extensions;
using Trunk.Logic.Dimensions.Frequencies;
using Trunk.Logic.Loaders;
using Trunk.Logic.Parsers;

namespace Trunk.App.Dimensions;

public class MeasureAuthorsLineAddedSettings : CommandSettings
{
    [Description("Specifies path to the git log file produced by this git command: 'git log --pretty=format:'[%h] %an %ad %s' --date=short --numstat --no-merges'")]
    [CommandArgument(0, "[git-log-file-path]")]
    public string? GitLogFilePath { get; set; }

    public override ValidationResult Validate()
    {
        return GitLogFilePath.ValidateFileArgument(nameof(GitLogFilePath));
    }
}

public class MeasureAuthorsLineAddedCommand : AsyncCommand<MeasureAuthorsLineAddedSettings>
{
    private const string OutputFileName = "lines-added-by-author.csv";
    
    public override async Task<int> ExecuteAsync(CommandContext context, MeasureAuthorsLineAddedSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Measuring...", async ctx =>
        {
            ctx.SetupSpinner();

            var validationResult = settings.Validate();
            if (!validationResult.Successful)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), validationResult.Message);
            }
            
            ctx.Status("Loading git log file");
            var revisions = await LoadRevisions(settings.GitLogFilePath!);

            ctx.Status("Counting file lines added by author");
            var measure = AuthorsCodeLinesAddedMeasurement.Measure(revisions);

            var model = ToKnowledgeNodeModel(measure);
            
            ctx.Status("Saving result to output");
            await OutputFileName.WriteToCsvFile(model);
        });
        
        AnsiConsole.WriteLine($"Measurement finished. Results exported to: '{OutputFileName}'");

        return 0;
    }

    private static IEnumerable<LinesAddedByAuthorModel> ToKnowledgeNodeModel(List<AuthorsCodeLinesAdded> authors)
    {
        return (from authorsCodeLinesAdded in authors
            from directoryCodeLinesAdded in authorsCodeLinesAdded.CodeAddedToDirs
            select new LinesAddedByAuthorModel
            {
                Author = authorsCodeLinesAdded.Author, 
                Directory = directoryCodeLinesAdded.Path, 
                NumberOfLines = directoryCodeLinesAdded.NumberOfLinesAdded
            }).ToList();
    }

    private static async Task<List<Revision>> LoadRevisions(string gitLogFilePath)
    {
        var loader = new FileSourceControlLogLoader(gitLogFilePath);
        using var streamReader = await loader.LoadAsync();
        var revisions = await GitRevisionParser.ParseAsync(streamReader);
        return revisions;
    }
}