using Spectre.Console.Cli;
using Trunk.App.Analysis;
using Trunk.App.Dimensions;
using Trunk.App.Visualisations.D3s;

var app = new CommandApp();
app.Configure(config =>
{
    config.PropagateExceptions();
    config.AddBranch("dimensions", dimensions =>
    {
        dimensions.AddBranch("measure", measure =>
        {
            measure.AddCommand<MeasureCodeLinesCommand>("lines-of-code");
            measure.AddCommand<MeasureFrequencyOfChangesCommand>("frequency-of-changes");
            measure.AddCommand<MeasureAuthorsLineAddedCommand>("lines-added-by-author");
        });
    });
    config.AddBranch("analyze", analyze =>
    {
        analyze.AddCommand<AnalyzeHotSpotsCommand>("hot-spots");
        analyze.AddCommand<AnalyzeWordMapCommand>("word-map");
    });
    
    config.AddBranch("visualize", visualizations =>
    {
        visualizations.AddCommand<CreateHotSpotDiagramCommand>("d3-hotspots");
        visualizations.AddCommand<CreateKnowledgeMapDiagramCommand>("d3-knowledgemap");
    });
});

return await app.RunAsync(args);