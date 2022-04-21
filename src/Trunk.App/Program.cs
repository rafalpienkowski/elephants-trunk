﻿using Spectre.Console.Cli;
using Trunk.App.Analysis.HotSpots;
using Trunk.App.Dimensions.Complexities;
using Trunk.App.Dimensions.Frequencies;

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
        });
    });
    config.AddBranch("analyze", analyze =>
    {
        analyze.AddCommand<AnalyzeHotSpotsCommand>("hot-spots");
    });
    
});

return await app.RunAsync(args);