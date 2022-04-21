// See https://aka.ms/new-console-template for more information

using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.PropagateExceptions();
    config.AddBranch<CalculateSettings>("calculate", calculate =>
    {
        calculate.AddCommand<CalculateHotSpotsCommand>("hotspots")
            .WithDescription("Calculates hot spots and export them to CSV file");
    });
});

return await app.RunAsync(args);